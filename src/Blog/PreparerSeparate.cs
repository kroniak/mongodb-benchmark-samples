using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using MongoDB.Bson;
using MongodbTransactions.Blog.MongodbModels;
using MongodbTransactions.Blog.SqlModels;
using Newtonsoft.Json;

namespace MongodbTransactions.Blog
{
    public class PreparerSeparate
    {
        private readonly int _count;
        private readonly Faker _faker;
        private readonly List<string> _userNames;
        private ConcurrentBag<UserMn> _usersMnRowData;
        private List<ArticleMn> _articlesMnRowData;
        private List<CommentMn> _commentsMnRowData;
        private List<CommentSql> _commentSqlRowData;
        private List<ArticleSql> _articlesSqlRowData;
        private List<UserSql> _usersSqlRowData;

        public PreparerSeparate(int count,
            Faker faker, List<string> userNames,
            List<UserSql> usersSqlRowData,
            ConcurrentBag<UserMn> usersMnRowData,
            List<ArticleSql> articlesSqlRowData,
            List<ArticleMn> articlesMnRowData,
            List<CommentMn> commentMnRowData,
            List<CommentSql> commentSqlRowData)
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

                    Parallel.ForEach(articles, (article, pls, index) =>
                    {
                        var comments = new Faker<CommentMn>("ru")
                            .RuleFor(u => u.Id, f => ObjectId.GenerateNewId().ToString())
                            .RuleFor(u => u.Text, f => f.Lorem.Text())
                            .RuleFor(u => u.Created, f => f.Date.Recent())
                            .RuleFor(u => u.ArticleId, article.Id)
                            .RuleFor(u => u.UserId, f => f.PickRandom(userIds))
                            .Generate(_faker.Random.Number(0, 10));

                        _commentsMnRowData.AddRange(comments);

                        var articleId = index + _faker.Random.Number(0, _count * 10);
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
                            UserId = _faker.Random.Number(0, _count),
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

        public bool LoadData()
        {
            Console.Write("Docs loading started...........");
            var sw = Stopwatch.StartNew();
            try
            {
                using (var file = File.OpenText(@"usersMnRowData2.json"))
                {
                    _usersMnRowData =
                        (ConcurrentBag<UserMn>) new JsonSerializer().Deserialize(file,
                            typeof(List<UserMn>));
                }

                using (var file = File.OpenText(@"articlesMnRowData2.json"))
                {
                    _articlesMnRowData =
                        (List<ArticleMn>) new JsonSerializer().Deserialize(file,
                            typeof(List<ArticleMn>));
                }

                using (var file = File.OpenText(@"commentsMnRowData2.json"))
                {
                    _commentsMnRowData =
                        (List<CommentMn>) new JsonSerializer().Deserialize(file,
                            typeof(List<CommentMn>));
                }

                using (var file = File.OpenText(@"articlesRowData2.json"))
                {
                    _articlesSqlRowData =
                        (List<ArticleSql>) new JsonSerializer().Deserialize(file, typeof(List<ArticleSql>));
                }

                using (var file = File.OpenText(@"usersRowData2.json"))
                {
                    _usersSqlRowData = (List<UserSql>) new JsonSerializer().Deserialize(file, typeof(List<UserSql>));
                }

                using (var file = File.OpenText(@"commentsRowData2.json"))
                {
                    _commentSqlRowData =
                        (List<CommentSql>) new JsonSerializer().Deserialize(file, typeof(List<CommentSql>));
                }

                // store user name for future usage
                _userNames.AddRange(_usersMnRowData.Select(u => u.Name));

                return true;
            }
            catch
            {
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
                File.WriteAllText(@"usersMnRowData2.json", JsonConvert.SerializeObject(_usersMnRowData));
                File.WriteAllText(@"articlesMnRowData2.json", JsonConvert.SerializeObject(_articlesMnRowData));
                File.WriteAllText(@"commentsMnRowData2.json", JsonConvert.SerializeObject(_commentsMnRowData));
                File.WriteAllText(@"articlesRowData2.json", JsonConvert.SerializeObject(_articlesSqlRowData));
                File.WriteAllText(@"usersRowData2.json", JsonConvert.SerializeObject(_usersSqlRowData));
                File.WriteAllText(@"commentsRowData2.json", JsonConvert.SerializeObject(_commentSqlRowData));
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