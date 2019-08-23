using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Bogus;
using FluentAssertions;
using MongoDB.Driver;
using MongodbTransactions.Blog.MongodbModels;
using MongodbTransactions.Blog.SqlModels;
using MongodbTransactions.Utils;
using Npgsql;

#pragma warning disable 618

namespace MongodbTransactions.Blog
{
    [SimpleJob(RunStrategy.Throughput, targetCount: 30, warmupCount: 3)]
    public class BlogSeparateDocuments
    {
        private IMongoCollection<UserMn> _users;
        private IMongoCollection<ArticleMn> _articles;
        private IMongoCollection<CommentMn> _comments;
        private IMongoDatabase _database;

        private const int Count = 1000;

        private readonly ConcurrentBag<UserMn> _usersMnRowData = new ConcurrentBag<UserMn>();
        private readonly ConcurrentBag<ArticleMn> _articlesMnRowData = new ConcurrentBag<ArticleMn>();
        private readonly ConcurrentBag<CommentMn> _commentsMnRowData = new ConcurrentBag<CommentMn>();

        private readonly ConcurrentBag<UserSql> _usersSqlRowData = new ConcurrentBag<UserSql>();
        private readonly ConcurrentBag<ArticleSql> _articlesSqlRowData = new ConcurrentBag<ArticleSql>();
        private readonly ConcurrentBag<CommentSql> _commentSqlRowData = new ConcurrentBag<CommentSql>();

        private readonly ConcurrentBag<string> _userNames = new ConcurrentBag<string>();

        private readonly Faker _faker = new Faker("ru");
        private PreparerSeparate _preparer;

        [GlobalSetup]
        public void Setup()
        {
            _preparer = new PreparerSeparate(
                Count,
                _faker,
                _userNames,
                _usersSqlRowData,
                _usersMnRowData,
                _articlesSqlRowData,
                _articlesMnRowData,
                _commentsMnRowData,
                _commentSqlRowData);
            
            _database = new MongoClient(GeneralUtils.MongodbLocalhost)
                .GetDatabase("blog_separate");

            Console.WriteLine("Start Global Blog Setup");
            if (!_preparer.LoadData())
            {
                _preparer.PrepareDocs();
                _preparer.SaveData();
            }
            else
            {
                GlobalCleanup();
                OpenMongodb();
                Console.WriteLine("Inserting docs started..........");
                InsertMongoDocs();
                InsertPostgresDocs();
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
            _database.DropCollection("comments");
        }

        private void OpenMongodb()
        {
            _users = _database.GetCollection<UserMn>("users");
            _articles = _database.GetCollection<ArticleMn>("articles");
            _comments = _database.GetCollection<CommentMn>("comments");

            _users.Indexes.CreateOne(new JsonIndexKeysDefinition<UserMn>("{\"Name\" : 1 }"));
            _articles.Indexes.CreateOne(new JsonIndexKeysDefinition<ArticleMn>("{\"UserId\" : 1 }"));
            _comments.Indexes.CreateOne(new JsonIndexKeysDefinition<CommentMn>("{\"UserId\" : 1 }"));
            Console.WriteLine("Opened mongo connection");
        }

        private void InsertMongoDocs()
        {
            Console.Write("Start inserted into mongodb...........");
            var sw = Stopwatch.StartNew();
            _users.BulkWrite(_usersMnRowData.Select(u => new InsertOneModel<UserMn>(u)));
            _articles.BulkWrite(_articlesMnRowData.Select(a => new InsertOneModel<ArticleMn>(a)));
            _comments.BulkWrite(_commentsMnRowData.Select(c => new InsertOneModel<CommentMn>(c)));
            Console.WriteLine("done at " + sw.ElapsedMilliseconds + " ms.");
        }

        private void InsertPostgresDocs()
        {
            Console.Write("Start inserted into postgres...........");
            var sw = Stopwatch.StartNew();
            using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
            {
                conn.Open();

                foreach (var document in _usersSqlRowData)
                {
                    using (var userCmd =
                        new NpgsqlCommand("INSERT INTO users (id, name, created, url) VALUES (@id,@n,@c,@u)",
                            conn))
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

                        using (var articleCmd =
                            new NpgsqlCommand(
                                "INSERT INTO articles (id, name, created, url, text, userid) VALUES (@id,@n,@c,@u,@t,@user)",
                                conn))
                        {
                            articleCmd.Parameters.AddWithValue("id", document.Id);
                            articleCmd.Parameters.AddWithValue("n", document.Name);
                            articleCmd.Parameters.AddWithValue("c", document.Created);
                            articleCmd.Parameters.AddWithValue("u", document.Url);
                            articleCmd.Parameters.AddWithValue("t", document.Text);
                            articleCmd.Parameters.AddWithValue("user", document.UserId);
                            try
                            {
                                articleCmd.ExecuteNonQuery();
                            }
                            catch
                            {
                            }
                        }
                    }
                });

            Parallel.ForEach(_commentSqlRowData,
                new ParallelOptions {MaxDegreeOfParallelism = 8},
                document =>
                {
                    using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
                    {
                        conn.Open();

                        using (var articleCmd =
                            new NpgsqlCommand(
                                "INSERT INTO comments (created, text, userid, articleid) VALUES (@c,@t,@user,@article)",
                                conn))
                        {
                            articleCmd.Parameters.AddWithValue("c", document.Created);
                            articleCmd.Parameters.AddWithValue("t", document.Text);
                            articleCmd.Parameters.AddWithValue("user", document.UserId);
                            articleCmd.Parameters.AddWithValue("article", document.ArticleId);
                            articleCmd.ExecuteNonQuery();
                        }
                    }
                });

            Console.Write("done at " + sw.ElapsedMilliseconds + " ms.............");

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
                          CREATE INDEX articles_comments_comments_1_index ON articles_comments USING GIN ((comments -> 'UserId'));
                          create index articles_userid_index on articles (userid);
	                      create index comments_userid_index on comments (userid);
                          create index comments_articleid_index on comments (articleid);
                          ";
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("done Indexes created into postgres at " + sw.ElapsedMilliseconds + " ms.");
        }

        [Benchmark]
        public void MongoSelectCommentsByUserName()
        {
            var userFilter = Builders<UserMn>.Filter.Eq(u => u.Name, _faker.PickRandom(_userNames.ToArray()));
            var user = _users.Find(userFilter).FirstOrDefault();

            var commentsFilter = Builders<CommentMn>.Filter
                .Where(c => c.UserId == user.Id);

            var comments = _comments.Find(commentsFilter).ToList();

            Parallel.ForEach(comments, comment =>
            {
                var articlesFilter = Builders<ArticleMn>.Filter
                    .Where(a => a.Id == comment.ArticleId);
                var article = _articles.Find(articlesFilter).FirstOrDefault();
                article.Should().NotBeNull();
            });
        }

        [Benchmark]
        public void PostgresSelectCommentsByUserName()
        {
            using (var conn = new NpgsqlConnection(GeneralUtils.PgConnectionString))
            {
                conn.Open();

                using (var cmd =
                    new NpgsqlCommand(
                        @"select a.id, a.name, a.url, c.id, c.text, c.created
                            from comments as c
                             join users u on c.userid = u.id
                             join articles a on c.articleid = a.id
                            where u.name =@name;", conn))
                {
                    cmd.Parameters.AddWithValue("@name", _faker.PickRandom(_userNames.ToArray()));
                    using (var reader = cmd.ExecuteReader())
                    {
                        var articles = reader.Select(r => new
                        {
                            Id = r.GetInt64(0),
                            Name = r.GetString(1),
                            Url = r.GetString(2),
                            Cid=r.GetInt64(3),
                            Text = r.GetString(4),
                            Created = r.GetDateTime(5),
                        });
                        articles.Should().NotBeEmpty();
                    }
                }
            }
        }
    }
}