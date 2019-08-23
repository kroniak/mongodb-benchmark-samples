using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using MongoDB.Bson;
using MongodbTransactions.Blog.MongodbModels;
using MongodbTransactions.Blog.SqlModels;
using MongodbTransactions.Utils;
using Newtonsoft.Json;

namespace MongodbTransactions.Blog
{
    public class PreparerSeparate
    {
        private readonly int _count;
        private readonly Faker _faker;
        private readonly ConcurrentBag<string> _userNames;
        private static long _counter;
        private ConcurrentBag<UserMn> _usersMnRowData;
        private ConcurrentBag<ArticleMn> _articlesMnRowData;
        private ConcurrentBag<CommentMn> _commentsMnRowData;
        private ConcurrentBag<CommentSql> _commentSqlRowData;
        private ConcurrentBag<ArticleSql> _articlesSqlRowData;
        private ConcurrentBag<UserSql> _usersSqlRowData;
        private ConcurrentBag<long> _ids = new ConcurrentBag<long>();

        public PreparerSeparate(int count,
            Faker faker,
            ConcurrentBag<string> userNames,
            ConcurrentBag<UserSql> usersSqlRowData,
            ConcurrentBag<UserMn> usersMnRowData,
            ConcurrentBag<ArticleSql> articlesSqlRowData,
            ConcurrentBag<ArticleMn> articlesMnRowData,
            ConcurrentBag<CommentMn> commentMnRowData,
            ConcurrentBag<CommentSql> commentSqlRowData)
        {
            _count = count;
            _faker = faker ?? throw new ArgumentNullException(nameof(faker));
            _userNames = userNames ?? throw new ArgumentNullException(nameof(userNames));
            _usersSqlRowData = usersSqlRowData ?? throw new ArgumentNullException(nameof(usersSqlRowData));
            _usersMnRowData = usersMnRowData ?? throw new ArgumentNullException(nameof(usersMnRowData));
            _articlesSqlRowData = articlesSqlRowData ?? throw new ArgumentNullException(nameof(articlesSqlRowData));
            _articlesMnRowData = articlesMnRowData ?? throw new ArgumentNullException(nameof(articlesMnRowData));
            _commentsMnRowData = commentMnRowData ?? throw new ArgumentNullException(nameof(commentMnRowData));
            _commentSqlRowData = commentSqlRowData ?? throw new ArgumentNullException(nameof(commentSqlRowData));
        }

        public void PrepareDocs()
        {
            Console.Write("Preparing docs started..........");
            var sw = Stopwatch.StartNew();
            var userIds = new List<string>(_count);

            // generate main data 
            Parallel.ForEach(Enumerable.Range(0, _count),
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = 10
                }, i =>
                {
                    var gender = _faker.PickRandom<Name.Gender>();
                    var id = ObjectId.GenerateNewId().ToString();
                    userIds.Add(id);

                    var user = new Faker<UserMn>("ru")
                        .RuleFor(u => u.Id, id)
                        .RuleFor(u => u.Name, f =>
                            f.Name.FirstName(gender) + " " + f.Name.LastName(gender) + " " + i)
                        .RuleFor(u => u.Url, f => f.Internet.Url())
                        .RuleFor(u => u.Created, f => f.Date.Recent())
                        .Generate();

                    // store user name for future usage
                    _userNames.Add(user.Name);

                    var articles = new Faker<ArticleMn>("ru")
                        .RuleFor(u => u.Id, f => ObjectId.GenerateNewId().ToString())
                        .RuleFor(u => u.UserId, id)
                        .RuleFor(u => u.Name, f => f.Lorem.Slug())
                        .RuleFor(u => u.Created, f => f.Date.Recent())
                        .RuleFor(u => u.Url, f => f.Internet.Url())
                        .RuleFor(u => u.Text, f => f.Lorem.Paragraphs())
                        .Generate(10);

                    // store row data for mongodb
                    _usersMnRowData.Add(user);
                    _articlesMnRowData.AddRange(articles);

                    Parallel.ForEach(articles, article =>
                    {
                        var comments = new Faker<CommentMn>("ru")
                            .RuleFor(u => u.Id, f => ObjectId.GenerateNewId().ToString())
                            .RuleFor(u => u.Text, f => f.Lorem.Text())
                            .RuleFor(u => u.Created, f => f.Date.Recent())
                            .RuleFor(u => u.ArticleId, article.Id)
                            .RuleFor(u => u.UserId, f => f.PickRandom(userIds))
                            .Generate(_faker.Random.Number(0, 10));

                        _commentsMnRowData.AddRange(comments);

                        var articleId = GetUniqInt();

                        _articlesSqlRowData.Add(new ArticleSql
                        {
                            Id = articleId,
                            Name = article.Name,
                            Created = article.Created,
                            Url = article.Url,
                            Text = article.Text,
                            UserId = i
                        });

                        _commentSqlRowData.AddRange(comments.Select(c => new CommentSql
                        {
                            Created = c.Created,
                            Text = c.Text,
                            UserId = _faker.Random.Number(0, _count - 1),
                            ArticleId = articleId
                        }));
                    });

                    // generate row data for pg
                    _usersSqlRowData.Add(new UserSql
                    {
                        Id = i,
                        Created = user.Created,
                        Name = user.Name,
                        Url = user.Url
                    });
                });
            Console.WriteLine("done at " + sw.ElapsedMilliseconds + " ms");
        }

        private static long GetUniqInt()
        {
            object o = new { };

            lock (o)
            {
                _counter++;
                return _counter;
            }
        }

        public bool LoadData()
        {
            Console.Write("Docs loading started...........");
            var sw = Stopwatch.StartNew();
            try
            {
                using (var file = File.OpenText(@"/tmp/usersMnRowData2.json"))
                {
                    _usersMnRowData.AddRange(
                        (ConcurrentBag<UserMn>) new JsonSerializer().Deserialize(file,
                            typeof(ConcurrentBag<UserMn>)));
                }

                using (var file = File.OpenText(@"/tmp/articlesMnRowData2.json"))
                {
                    _articlesMnRowData.AddRange(
                        (ConcurrentBag<ArticleMn>) new JsonSerializer().Deserialize(file,
                            typeof(ConcurrentBag<ArticleMn>)));
                }

                using (var file = File.OpenText(@"/tmp/commentsMnRowData2.json"))
                {
                    _commentsMnRowData.AddRange(
                        (ConcurrentBag<CommentMn>) new JsonSerializer().Deserialize(file,
                            typeof(ConcurrentBag<CommentMn>)));
                }

                using (var file = File.OpenText(@"/tmp/articlesRowData2.json"))
                {
                    _articlesSqlRowData.AddRange(
                        (ConcurrentBag<ArticleSql>) new JsonSerializer().Deserialize(file,
                            typeof(ConcurrentBag<ArticleSql>)));
                }

                using (var file = File.OpenText(@"/tmp/usersRowData2.json"))
                {
                    _usersSqlRowData.AddRange(
                        (ConcurrentBag<UserSql>) new JsonSerializer().Deserialize(file,
                            typeof(ConcurrentBag<UserSql>)));
                }

                using (var file = File.OpenText(@"/tmp/commentsRowData2.json"))
                {
                    _commentSqlRowData.AddRange(
                        (ConcurrentBag<CommentSql>) new JsonSerializer().Deserialize(file,
                            typeof(ConcurrentBag<CommentSql>)));
                }

                // store user name for future usage
                _userNames.AddRange(_usersMnRowData.Select(u => u.Name));

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                Console.WriteLine("done at " + sw.ElapsedMilliseconds + " ms");
            }
        }

        public void SaveData()
        {
            Console.Write("Docs saving started...........");
            var sw = Stopwatch.StartNew();
            try
            {
                File.WriteAllText(@"/tmp/usersMnRowData2.json", JsonConvert.SerializeObject(_usersMnRowData));
                File.WriteAllText(@"/tmp/articlesMnRowData2.json", JsonConvert.SerializeObject(_articlesMnRowData));
                File.WriteAllText(@"/tmp/commentsMnRowData2.json", JsonConvert.SerializeObject(_commentsMnRowData));
                File.WriteAllText(@"/tmp/articlesRowData2.json", JsonConvert.SerializeObject(_articlesSqlRowData));
                File.WriteAllText(@"/tmp/usersRowData2.json", JsonConvert.SerializeObject(_usersSqlRowData));
                File.WriteAllText(@"/tmp/commentsRowData2.json", JsonConvert.SerializeObject(_commentSqlRowData));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine("done at " + sw.ElapsedMilliseconds + " ms");
        }
    }
}