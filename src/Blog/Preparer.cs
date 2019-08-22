using System;
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
    public class Preparer
    {
        private readonly int _count;
        private readonly Faker _faker;
        private readonly List<string> _userNames;
        private List<UserSql> _usersSqlRowData;
        private List<UserMn> _usersMnRowData;
        private List<ArticleSql> _articlesSqlRowData;
        private List<ArticleMn> _articlesMnRowData;

        public Preparer(int count, Faker faker, List<string> userNames, List<UserSql> usersSqlRowData,
            List<UserMn> usersMnRowData, List<ArticleSql> articlesSqlRowData, List<ArticleMn> articlesMnRowData)
        {
            _count = count;
            _faker = faker ?? throw new ArgumentNullException(nameof(faker));
            _userNames = userNames ?? throw new ArgumentNullException(nameof(userNames));
            _usersSqlRowData = usersSqlRowData ?? throw new ArgumentNullException(nameof(usersSqlRowData));
            _usersMnRowData = usersMnRowData ?? throw new ArgumentNullException(nameof(usersMnRowData));
            _articlesSqlRowData = articlesSqlRowData ?? throw new ArgumentNullException(nameof(articlesSqlRowData));
            _articlesMnRowData = articlesMnRowData ?? throw new ArgumentNullException(nameof(articlesMnRowData));
        }

        public void PrepareSc1Docs()
        {
            Console.Write("Preparing docs started..........");
            var sw = Stopwatch.StartNew();
            var userIds = new List<string>(_count / 100);

            // generate main data 
            Parallel.ForEach(Enumerable.Range(0, _count / 100), i =>
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

                var testComment = new Faker<CommentMn>("ru")
                    .RuleFor(u => u.Text, f => f.Lorem.Text())
                    .RuleFor(u => u.Created, f => f.Date.Recent())
                    .RuleFor(u => u.UserId, f => f.PickRandom(userIds));

                var articles = new Faker<ArticleMn>("ru")
                    .RuleFor(u => u.Id, f => ObjectId.GenerateNewId().ToString())
                    .RuleFor(u => u.UserId, id)
                    .RuleFor(u => u.Name, f => f.Lorem.Slug())
                    .RuleFor(u => u.Created, f => f.Date.Recent())
                    .RuleFor(u => u.Url, f => f.Internet.Url())
                    .RuleFor(u => u.Text, f => f.Lorem.Paragraphs())
                    .RuleFor(u => u.Comments, f => testComment.Generate(f.Random.Number(1, 10)))
                    .Generate(_count / 1000);

                user.Articles = articles.Select(a => a.Id).ToArray();

                // store row data for mongodb
                _usersMnRowData.Add(user);
                _articlesMnRowData.AddRange(articles);

                // generate row data for pg
                var testCommentPg = new Faker<CommentSql>("ru")
                    .RuleFor(u => u.Text, f => f.Lorem.Text())
                    .RuleFor(u => u.Created, f => f.Date.Recent())
                    .RuleFor(u => u.UserId, f => f.Random.Number(1, _count / 100));

                _articlesSqlRowData.AddRange(articles.Select(a => new ArticleSql
                {
                    Name = a.Name,
                    Created = a.Created,
                    Url = a.Url,
                    Text = a.Text,
                    Comments = testCommentPg.Generate(_faker.Random.Number(1, 10)),
                    UserId = i
                }));

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

        public bool LoadSc1Data()
        {
            Console.Write("Docs loading started...........");
            var sw = Stopwatch.StartNew();
            try
            {
                using (var file = File.OpenText(@"usersMnRowData.json"))
                {
                    _usersMnRowData =
                        (List<UserMn>) new JsonSerializer().Deserialize(file,
                            typeof(List<UserMn>));
                }

                using (var file = File.OpenText(@"articlesMnRowData.json"))
                {
                    _articlesMnRowData =
                        (List<ArticleMn>) new JsonSerializer().Deserialize(file,
                            typeof(List<ArticleMn>));
                }

                using (var file = File.OpenText(@"articlesRowData.json"))
                {
                    _articlesSqlRowData =
                        (List<ArticleSql>) new JsonSerializer().Deserialize(file, typeof(List<ArticleSql>));
                }

                using (var file = File.OpenText(@"usersRowData.json"))
                {
                    _usersSqlRowData = (List<UserSql>) new JsonSerializer().Deserialize(file, typeof(List<UserSql>));
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

        public void SaveSc1Data()
        {
            Console.Write("Docs saving started...........");
            var sw = Stopwatch.StartNew();
            File.WriteAllText(@"usersMnRowData.json", JsonConvert.SerializeObject(_usersMnRowData));
            File.WriteAllText(@"articlesMnRowData.json", JsonConvert.SerializeObject(_articlesMnRowData));
            File.WriteAllText(@"articlesRowData.json", JsonConvert.SerializeObject(_articlesSqlRowData));
            File.WriteAllText(@"usersRowData.json", JsonConvert.SerializeObject(_usersSqlRowData));
            Console.WriteLine("done at " + sw.ElapsedMilliseconds + " ms");
        }
    }
}