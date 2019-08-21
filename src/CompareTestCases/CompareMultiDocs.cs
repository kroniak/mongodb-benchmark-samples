using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using MongoDB.Driver;
using MongodbTransactions.CompareTestCases.Models;
using Npgsql;
using NpgsqlTypes;

namespace MongodbTransactions.CompareTestCases
{
    [CoreJob]
    public class CompareMultiDocs
    {
        private IEnumerable<TestModel> _documents;

        private const string SqlConnectionString =
            "Host=docker;Username=postgres;Password=mysecretpassword;Database=postgres";

        private const string MongoDbConnectionString = "mongodb://localhost:27017";

        [Params(10, 50, 100)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            CleanMongoDb();
            CleanPostgresDb();
            Console.WriteLine("Deleted Rows!!!");
            PrepareDocs();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            CleanMongoDb();
            CleanPostgresDb();
            Console.WriteLine("Deleted Rows!!!");
        }

        private static void CleanMongoDb()
        {
            var database = new MongoClient(MongoDbConnectionString).GetDatabase("foo");
            database.DropCollection("bar");
        }

        private static void CleanPostgresDb()
        {
            using (var conn = new NpgsqlConnection(SqlConnectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                        "TRUNCATE TABLE test";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void PrepareDocs()
        {
            _documents = Enumerable.Range(0, Count).Select(i => new TestModel
            {
                Counter = i,
                Name = "MongoDB",
                Type = "Database",
                Count = 1,
                Info = new TestStruct {X = 203, Y = 102}
            });
            Console.WriteLine("Prepared Docs!!!");
        }

        [Benchmark]
        public void SavePostgres()
        {
            using (var conn = new NpgsqlConnection(SqlConnectionString))
            {
                conn.Open();
                conn.TypeMapper.UseJsonNet();

                foreach (var document in _documents)
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText =
                            $"INSERT INTO test (counter, name, type, count, info) VALUES (@c,@n,@t,@co,@i)";
                        cmd.Parameters.AddWithValue("c", document.Counter);
                        cmd.Parameters.AddWithValue("n", document.Name);
                        cmd.Parameters.AddWithValue("t", document.Type);
                        cmd.Parameters.AddWithValue("co", document.Count);
                        cmd.Parameters.Add(new NpgsqlParameter("i", NpgsqlDbType.Jsonb) {Value = document.Info});
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        [Benchmark]
        public void SavePostgresWithTransaction()
        {
            using (var conn = new NpgsqlConnection(SqlConnectionString))
            {
                conn.Open();
                conn.TypeMapper.UseJsonNet();
                var transaction = conn.BeginTransaction();

                foreach (var document in _documents)
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText =
                            $"INSERT INTO test (counter, name, type, count, info) VALUES (@c,@n,@t,@co,@i)";
                        cmd.Parameters.AddWithValue("c", document.Counter);
                        cmd.Parameters.AddWithValue("n", document.Name);
                        cmd.Parameters.AddWithValue("t", document.Type);
                        cmd.Parameters.AddWithValue("co", document.Count);
                        cmd.Parameters.Add(new NpgsqlParameter("i", NpgsqlDbType.Jsonb) {Value = document.Info});
                        cmd.ExecuteNonQuery();
                    }
                }

                transaction.Save("transaction");
            }
        }

        [Benchmark(Baseline = true)]
        public void SaveMongo()
        {
            SaveMongo(null);
        }

        private void SaveMongo(MongoClient client)
        {
            var cl = client ?? new MongoClient(MongoDbConnectionString);
            var collection = cl.GetDatabase("foo")
                .GetCollection<TestModel>("bar");
            collection.InsertMany(_documents);
        }

        [Benchmark]
        public void SaveMongoWithTransaction()
        {
            var client = new MongoClient(MongoDbConnectionString);
            using (var session = client.StartSession())
            {
                session.StartTransaction();
                SaveMongo(client);
                session.CommitTransaction();
            }
        }
    }
}