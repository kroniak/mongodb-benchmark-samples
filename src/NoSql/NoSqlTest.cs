using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Bogus;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongodbTransactions.Blog;
using MongodbTransactions.NoSql.Models;
using MongodbTransactions.Utils;
using Npgsql;

#pragma warning disable 618

namespace MongodbTransactions.NoSql
{
    [SimpleJob(RunStrategy.Throughput, targetCount: 30, warmupCount: 3)]
    public class NoSqlTest
    {
        private IMongoCollection<Package> _packages;
        private IMongoDatabase _database;

        private const int Count = 10000;

        private readonly List<Package> _packagesData = new List<Package>(Count);

        [GlobalSetup]
        public void Setup()
        {
            _database = new MongoClient(GeneralUtils.MongodbLocalhost)
                .GetDatabase("nosql");

            Console.WriteLine("Start Global Blog Setup");

            PrepareDocs();
            GlobalCleanup();
            OpenMongodb();
            Console.WriteLine("Inserting docs started..........");
            InsertMongoDocs();
            InsertPostgresDocs();
        }

        private void PrepareDocs()
        {
            Console.Write("Preparing docs started..........");
            var sw = Stopwatch.StartNew();

            // generate main data 
            Parallel.ForEach(Enumerable.Range(0, Count), new ParallelOptions()
            {
                MaxDegreeOfParallelism = 2
            }, i =>
            {
                var authRecord = new Faker<AuthService>("ru")
                    .RuleFor(s => s.Name, "AuthService")
                    .RuleFor(s => s.IsAuth, f => f.Random.Bool())
                    .RuleFor(s => s.UserName, f => f.Internet.Email())
                    .RuleFor(u => u.Created, f => f.Date.Recent())
                    .Generate();

                var billingRecord = new Faker<BillingService>("ru")
                    .RuleFor(s => s.Name, "BillingService")
                    .RuleFor(s => s.IsBilled, f => f.Random.Bool())
                    .RuleFor(s => s.Provider, f => f.Company.CompanyName())
                    .RuleFor(u => u.Created, f => f.Date.Recent())
                    .Generate();

                var partnerRecord = new Faker<PartnerService>("ru")
                    .RuleFor(s => s.Name, "PartnerService")
                    .RuleFor(s => s.IsDone, f => f.Random.Bool())
                    .RuleFor(s => s.Partner, f => f.Company.CompanyName())
                    .RuleFor(u => u.Created, f => f.Date.Recent())
                    .Generate();

                var package = new Faker<Package>("ru")
                    .RuleFor(u => u.InitSystem, f => f.Company.CompanyName())
                    .RuleFor(u => u.Records,
                        f => new List<ServiceRecord>(3) {authRecord, billingRecord, partnerRecord})
                    .RuleFor(u => u.Created, f => f.Date.Recent())
                    .Generate();

                _packagesData.Add(package);
            });

            Console.WriteLine("done at " + sw.ElapsedMilliseconds + " ms");
        }

        private void GlobalCleanup()
        {
            var sw = Stopwatch.StartNew();
            Console.Write("Start delete nosql Rows..........");
            CleanMongoDb();
            Cleaner.CleanPostgresDb();
            Console.WriteLine("done at " + sw.ElapsedMilliseconds + " ms.");
        }

        private void CleanMongoDb()
        {
            _database.DropCollection("packages");
        }

        private void OpenMongodb()
        {
            _packages = _database.GetCollection<Package>("packages");

//            _packages.Indexes.CreateOne(new JsonIndexKeysDefinition<Package>("{\"Records.Name\" : 1 }"));
//            _packages.Indexes.CreateOne(new JsonIndexKeysDefinition<Package>("{\"Records.Created\" : 1 }"));
            _packages.Indexes.CreateOne(new JsonIndexKeysDefinition<Package>("{\"Records.IsAuth\" : 1 }"));
            _packages.Indexes.CreateOne(new JsonIndexKeysDefinition<Package>("{\"Records.IsBilled\" : 1 }"));
            Console.WriteLine("Opened mongo connection");
        }

        private void InsertMongoDocs()
        {
            var sw = Stopwatch.StartNew();
            _packages.BulkWrite(_packagesData.Select(u => new InsertOneModel<Package>(u)));
            Console.WriteLine("Docs inserted into mongodb at " + sw.ElapsedMilliseconds + " ms.");
        }

        private void InsertPostgresDocs()
        {
            var sw = Stopwatch.StartNew();

            Parallel.ForEach(_packagesData,
                new ParallelOptions {MaxDegreeOfParallelism = 8},
                document =>
                {
                    using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
                    {
                        conn.Open();

                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;

                            foreach (var record in document.Records)
                            {
                                switch (record)
                                {
                                    case AuthService authService:
                                        cmd.CommandText =
                                            @"INSERT INTO packages (created, init_system, 
                                                record_name, 
                                                record_created, 
                                                record_is_auth,
                                                record_username
                                                ) VALUES (@c,@i,@rn,@rc,@ria,@ru)";
                                        cmd.Parameters.AddWithValue("c", document.Created);
                                        cmd.Parameters.AddWithValue("i", document.InitSystem);
                                        cmd.Parameters.AddWithValue("rn", record.Name);
                                        cmd.Parameters.AddWithValue("rc", record.Created);

                                        cmd.Parameters.AddWithValue("ria", authService.IsAuth);
                                        cmd.Parameters.AddWithValue("ru", authService.UserName);
                                        break;
                                    case BillingService billingService:
                                        cmd.CommandText =
                                            @"INSERT INTO packages (created, init_system, 
                                                record_name, 
                                                record_created, 
                                                record_is_billed,
                                                record_provider
                                                ) VALUES (@c,@i,@rn,@rc,@rib,@rpr)";
                                        cmd.Parameters.AddWithValue("c", document.Created);
                                        cmd.Parameters.AddWithValue("i", document.InitSystem);
                                        cmd.Parameters.AddWithValue("rn", record.Name);
                                        cmd.Parameters.AddWithValue("rc", record.Created);

                                        cmd.Parameters.AddWithValue("rib", billingService.IsBilled);
                                        cmd.Parameters.AddWithValue("rpr", billingService.Provider);
                                        break;
                                    case PartnerService partnerService:
                                        cmd.CommandText =
                                            @"INSERT INTO packages (created, init_system, 
                                                record_name, 
                                                record_created, 
                                                record_is_done,
                                                record_partner) VALUES (@c,@i,@rn,@rc,@rid,@rpa)";
                                        cmd.Parameters.AddWithValue("c", document.Created);
                                        cmd.Parameters.AddWithValue("i", document.InitSystem);
                                        cmd.Parameters.AddWithValue("rn", record.Name);
                                        cmd.Parameters.AddWithValue("rc", record.Created);

                                        cmd.Parameters.AddWithValue("rid", partnerService.IsDone);
                                        cmd.Parameters.AddWithValue("rpa", partnerService.Partner);
                                        break;
                                }
                            }
                            cmd.ExecuteNonQuery();
                        }
                    }
                });

            Console.WriteLine("Docs inserted into postgres at " + sw.ElapsedMilliseconds + " ms.");

            sw.Start();
            using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                        @"create index if not exists packages_init_system_index
	                        on packages (init_system);
                        create index if not exists packages_record_is_auth_index
	                        on packages (record_is_auth);
                        create index if not exists packages_record_is_billed_index
	                        on packages (record_is_billed);";
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Indices created into postgres at " + sw.ElapsedMilliseconds + " ms.");
        }

        [Benchmark]
        public void MongoSelectAuthAndBilledRecords()
        {
            var filter = new BsonDocument
            {
                {"Records.IsAuth", true},
                {"Records.IsBilled", true},
            };

            var packages = _packages.Find(filter).ToList();
            packages.Should().NotBeEmpty();
        }
        
        [Benchmark]
        public void PostgresSelectAuthAndBilledRecords()
        {
            using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(
                        @"select *
                            from packages p
                                join packages pa on p.init_system=pa.init_system
                                join packages pb on p.init_system=pb.init_system
                            where pa.record_is_auth =true
                            and pb.record_is_billed = true", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var packages = reader.Select(r => new
                        {
                            Id = r.GetInt64(0),
                            Created = r.GetDateTime(1),
                            System = r.GetString(2),
                            Name = r.GetString(3),
                        });
                        packages.Should().NotBeEmpty();
                    }
                }
            }
        }
    }
}