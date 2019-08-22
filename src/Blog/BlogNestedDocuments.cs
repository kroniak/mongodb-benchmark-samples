using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Loggers;
using Bogus;
using Bogus.DataSets;
using MongoDB.Bson;
using MongoDB.Driver;
using MongodbTransactions.Blog.MongodbModels;
using MongodbTransactions.Blog.SqlModels;
using MongodbTransactions.Utils;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;

#pragma warning disable 618

namespace MongodbTransactions.Blog
{
    [SimpleJob(RunStrategy.Throughput, targetCount: 30, warmupCount: 3)]
    public class BlogNestedDocuments
    {
        private IMongoCollection<UserMn> _users;
        private IMongoCollection<ArticleMn> _articles;
        private IMongoDatabase _database;

        private const int Count = 100000;

        private readonly List<UserSql> _usersSqlRowData = new List<UserSql>(Count / 100);
        private readonly List<UserMn> _usersMnRowData = new List<UserMn>(Count / 100);
        private readonly List<ArticleSql> _articlesSqlRowData = new List<ArticleSql>(Count);
        private readonly List<ArticleMn> _articlesMnRowData = new List<ArticleMn>(Count);

        private readonly List<string> _userNames = new List<string>(Count / 100);
        private readonly Faker _faker = new Faker("ru");
        private readonly Preparer _preparer;

        public BlogNestedDocuments()
        {
            _preparer = new Preparer(Count, _faker, _userNames, _usersSqlRowData, _usersMnRowData, _articlesSqlRowData,
                _articlesMnRowData);
        }

        [GlobalSetup]
        public void Setup()
        {
            _database = new MongoClient(GeneralUtils.MongodbLocalhost)
                .GetDatabase("blog");

            Console.WriteLine("Start Global Blog Setup");
            if (!_preparer.LoadSc1Data())
            {
                _preparer.PrepareSc1Docs();
                _preparer.SaveSc1Data();
                GlobalCleanup();
                OpenMongodb();
                Console.WriteLine("Inserting docs started..........");
                InsertMongoDocs();
                InsertPostgresDocs();
            }
            else
            {
                OpenMongodb();
            }
        }

        private void GlobalCleanup()
        {
            var sw = Stopwatch.StartNew();
            Console.Write("Start delete Blog Rows..........");
            CleanMongoDb();
            Cleaner.CleanPostgresDb();
            Console.WriteLine("done at " + sw.ElapsedMilliseconds + " ms.");
        }

        private void CleanMongoDb()
        {
            _database.DropCollection("users");
            _database.DropCollection("articles");
        }

        private void OpenMongodb()
        {
            _users = _database.GetCollection<UserMn>("users");
            _articles = _database.GetCollection<ArticleMn>("articles");

            _users.Indexes.CreateOne(new JsonIndexKeysDefinition<UserMn>("{\"Name\" : 1 }"));
//            _articles.Indexes.CreateOne(new JsonIndexKeysDefinition<ArticleMn>("{\"Created\" : 1 }"));
            _articles.Indexes.CreateOne(new JsonIndexKeysDefinition<ArticleMn>("{\"Comments.UserId\" : 1 }"));
            _articles.Indexes.CreateOne(new JsonIndexKeysDefinition<ArticleMn>("{\"Comments.Text\" : 1 }"));
            Console.WriteLine("Opened mongo connection");
        }


        private void InsertMongoDocs()
        {
            var sw = Stopwatch.StartNew();
            _users.BulkWrite(_usersMnRowData.Select(u => new InsertOneModel<UserMn>(u)));
            _articles.BulkWrite(_articlesMnRowData.Select(a => new InsertOneModel<ArticleMn>(a)));
            Console.WriteLine("Docs inserted into mongodb at " + sw.ElapsedMilliseconds + " ms.");
        }

        private void InsertPostgresDocs()
        {
            var sw = Stopwatch.StartNew();
            using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
            {
                conn.Open();
                // prepare command
                using (var userCmd =
                    new NpgsqlCommand("INSERT INTO users (id, name, created, url) VALUES (@id,@n,@c,@u)", conn))
                {
                    userCmd.Parameters.Add("id", NpgsqlDbType.Bigint);
                    userCmd.Parameters.Add("n", NpgsqlDbType.Varchar);
                    userCmd.Parameters.Add("c", NpgsqlDbType.Date);
                    userCmd.Parameters.Add("u", NpgsqlDbType.Varchar);
                    userCmd.Prepare();
                }

                using (var articleCmd =
                    new NpgsqlCommand(
                        "INSERT INTO articles_comments (name, created, url, text, userid, comments) VALUES (@n,@c,@u,@t,@user,@co)",
                        conn))
                {
                    articleCmd.Parameters.Add("n", NpgsqlDbType.Varchar);
                    articleCmd.Parameters.Add("c", NpgsqlDbType.Date);
                    articleCmd.Parameters.Add("u", NpgsqlDbType.Varchar);
                    articleCmd.Parameters.Add("t", NpgsqlDbType.Varchar);
                    articleCmd.Parameters.Add("user", NpgsqlDbType.Bigint);
                    articleCmd.Parameters.Add("co", NpgsqlDbType.Jsonb);
                    articleCmd.Prepare();
                }
            }

            using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
            {
                conn.Open();

                foreach (var document in _usersSqlRowData)
                {
                    using (var userCmd =
                        new NpgsqlCommand("INSERT INTO users (id, name, created, url) VALUES (@id,@n,@c,@u)", conn))
                    {
                        userCmd.Parameters.AddWithValue("id", document.Id);
                        userCmd.Parameters.AddWithValue("n", document.Name);
                        userCmd.Parameters.AddWithValue("c", document.Created);
                        userCmd.Parameters.AddWithValue("u", document.Url);
                        userCmd.ExecuteNonQuery();
                    }
                }
            }

            Parallel.ForEach(_articlesSqlRowData,
                new ParallelOptions {MaxDegreeOfParallelism = 8},
                document =>
                {
                    using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
                    {
                        conn.Open();
                        conn.TypeMapper.UseJsonNet();

                        using (var articleCmd =
                            new NpgsqlCommand(
                                "INSERT INTO articles_comments (name, created, url, text, userid, comments) VALUES (@n,@c,@u,@t,@user,@co)",
                                conn))
                        {
                            articleCmd.Parameters.AddWithValue("n", document.Name);
                            articleCmd.Parameters.AddWithValue("c", document.Created);
                            articleCmd.Parameters.AddWithValue("u", document.Url);
                            articleCmd.Parameters.AddWithValue("t", document.Text);
                            articleCmd.Parameters.AddWithValue("user", document.UserId);
                            articleCmd.Parameters.Add(new NpgsqlParameter("co", NpgsqlDbType.Jsonb)
                                {Value = document.Comments});
                            articleCmd.ExecuteNonQuery();
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
                        @"create index users_name_index on users (name);
                          create index articles_comments_userid_index on articles_comments (userid);
                          CREATE INDEX articles_comments_comments_index ON articles_comments USING GIN (comments);
                          CREATE INDEX articles_comments_comments_1_index ON articles_comments USING GIN ((comments -> 'UserId'));";
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Indices created into postgres at " + sw.ElapsedMilliseconds + " ms.");
        }
        
        [Benchmark]
        public void MongoSelectUserByUserName()
        {
            var userFilter = Builders<UserMn>.Filter.Eq(u => u.Name, _faker.PickRandom(_userNames));
            _users.Find(userFilter).FirstOrDefault();
        }

        [Benchmark]
        public void PostgresSelectUserByUserName()
        {
            using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
            {
                conn.Open();
                using (var cmd =
                    new NpgsqlCommand("select id, name, url from users where name=@name", conn))
                {
                    cmd.Parameters.AddWithValue("@name", _faker.PickRandom(_userNames));
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            var id = reader[0];
                        }
                }
            }
        }
        
        [Benchmark]
        public void MongoSelectCommentsByUserName()
        {
            var userFilter = Builders<UserMn>.Filter.Eq(u => u.Name, _faker.PickRandom(_userNames));
            var user = _users.Find(userFilter).FirstOrDefault();

            var articlesFilter = Builders<ArticleMn>.Filter
                .ElemMatch(a => a.Comments, comment => comment.UserId == user.Id);

            var projection = Builders<ArticleMn>.Projection
                .Include(a => a.Name)
                .Include(a => a.Url)
                .Include(a => a.Comments)
                .ElemMatch(a => a.Comments, comment => comment.UserId == user.Id);

            _articles.Find(articlesFilter).Project(projection).ToList();
        }

        [Benchmark]
        public void PostgresSelectCommentsByUserName()
        {
            using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
            {
                conn.Open();

                using (var cmd =
                    new NpgsqlCommand(
                        @"select t.id,t.name,t.url, t.v
                from (select id, name, url, userid, jsonb_array_elements(comments)::jsonb as v from articles_comments) as t
                join users as u on u.id=jsonb_extract_path_text(v,'UserId')::int
                where u.name=@name", conn))
                {
                    cmd.Parameters.AddWithValue("@name", _faker.PickRandom(_userNames));
                    using (var reader = cmd.ExecuteReader())
                    {
                        var articles = reader.Select(r => new ArticleSql
                        {
                            Id = r.GetInt64(0),
                            Name = r.GetString(1),
                            Url = r.GetString(2),
                            Comments = JsonConvert.DeserializeObject<List<CommentSql>>(
                                "[" + r.GetString(3) + "]")
                        });
                    }
                }
            }
        }

        [Benchmark]
        public void PostgresSelectCommentsByUserName2()
        {
            using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
            {
                conn.Open();

                long id;
                using (var cmd =
                    new NpgsqlCommand("select id from users where name=@name", conn))
                {
                    cmd.Parameters.AddWithValue("@name", _faker.PickRandom(_userNames));
                    id = (long) cmd.ExecuteScalar();
                }

                using (var cmd =
                    new NpgsqlCommand(
                        "select t.id, t.name, t.url, t.comments from articles_comments as t where t.comments @> '[{\"UserId\": " +
                        id +
                        "}]';",
                        conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var comments = reader.Select(r => new ArticleSql
                        {
                            Id = r.GetInt64(0),
                            Name = r.GetString(1),
                            Url = r.GetString(2),
                            Comments = JsonConvert.DeserializeObject<List<CommentSql>>(r.GetString(3))
                        }).SelectMany(a => a.Comments).Where(c => c.UserId == id).ToList();
                    }
                }
            }
        }
    }
}