using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using FluentAssertions;
using MongoDB.Driver;

namespace MongodbTransactions.ConcurrentTestCases
{
    [SimpleJob(RunStrategy.ColdStart, targetCount: 1)]
    public class MultiCollectionsConcurrentUpdate
    {
        private IMongoCollection<TestConcurrentModel> _collectionBar;
        private IMongoCollection<TestConcurrentModel> _collectionBaz;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public MultiCollectionsConcurrentUpdate()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("foo");
        }

        [Params( 100000)]
        public int Count { get; set; }

        [Params(5, 10, 20)]
        public int ThreadCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            CleanDb();
            InsertDocs();
        }

        private void CleanDb()
        {
            _database.DropCollection("bar");
            _database.DropCollection("baz");
            _collectionBar = _database.GetCollection<TestConcurrentModel>("bar");
            _collectionBaz = _database.GetCollection<TestConcurrentModel>("baz");

            _collectionBar.Indexes.CreateOne(new JsonIndexKeysDefinition<TestConcurrentModel>("{\"Spread\" : 1 }"));
            _collectionBar.Indexes.CreateOne(new JsonIndexKeysDefinition<TestConcurrentModel>("{\"LastUpdater\" : 1 }"));
            _collectionBaz.Indexes.CreateOne(new JsonIndexKeysDefinition<TestConcurrentModel>("{\"Spread\" : 1 }"));
            _collectionBaz.Indexes.CreateOne(new JsonIndexKeysDefinition<TestConcurrentModel>("{\"LastUpdater\" : 1 }"));
            
            Console.WriteLine("Deleted Rows!!!");
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            CleanDb();
        }

        private void InsertDocs()
        {
            var rand = new Random();
            
            var bulkBar = new List<WriteModel<TestConcurrentModel>>();
            var bulkBaz = new List<WriteModel<TestConcurrentModel>>();
            
            foreach (var i in Enumerable.Range(0, Count))
            {
                bulkBar.Add(new InsertOneModel<TestConcurrentModel>(new TestConcurrentModel
                {
                    Spread = i % 12,
                    Additional = new TestStruct
                    {
                        X = rand.Next(255),
                        Y = rand.Next(255)
                    }
                }));
                bulkBaz.Add(new InsertOneModel<TestConcurrentModel>(new TestConcurrentModel
                {
                    Spread = i % 12,
                    Additional = new TestStruct
                    {
                        X = rand.Next(255),
                        Y = rand.Next(255)
                    }
                }));
            }

            _collectionBar.BulkWrite(bulkBar);
            _collectionBaz.BulkWrite(bulkBaz);
        }

        private void UpdateDocs()
        {
            var update = Builders<TestConcurrentModel>.Update
                .Set(d => d.Counter, 0)
                .Set(d => d.LastUpdater, null);

            _collectionBar.UpdateMany(_ => true, update);
            _collectionBaz.UpdateMany(_ => true, update);
        }

        [Benchmark]
        public void SaveAndUpdateWithTransaction()
        {
//            UpdateDocs();

            var tasks = new List<Task>();
            foreach (var i in Enumerable.Range(1, ThreadCount))
            {
                var task = Task.Factory.StartNew(() =>
                {
                    using (var session = _client.StartSession())
                    {
                        session.StartTransaction();
                        SelectAndUpdate(_collectionBar, i, "Thread#" + i);
                        SelectAndUpdate(_collectionBaz, i, "Thread#" + i);
                        session.CommitTransaction();
                    }
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            var sum = Enumerable.Range(1, ThreadCount).Sum();
            CheckUpdate(_collectionBar, sum);
            CheckUpdate(_collectionBaz, sum);
        }

        /// <summary>
        /// Eq
        ///     select * from foo
        ///     where Spread> Count%12
        /// 
        ///     update foo
        ///     set counter
        ///     value xxx
        ///     where id in (...)
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="step"></param>
        /// <param name="updaterName"></param>
        private void SelectAndUpdate(
            IMongoCollection<TestConcurrentModel> collection,
            int step,
            string updaterName)
        {
            var filterSelect = Builders<TestConcurrentModel>.Filter.Eq(d => d.Spread, Count % 12);
            var documents = collection.Find(filterSelect).Limit(100).ToList().Select(d => d.Id);

            var bulk = new List<WriteModel<TestConcurrentModel>>();

            var filter = Builders<TestConcurrentModel>.Filter.In(d => d.Id, documents);
            var update = Builders<TestConcurrentModel>.Update
                .Inc(d => d.Counter, step)
                .Set(d => d.LastUpdater, updaterName);
            bulk.Add(new UpdateManyModel<TestConcurrentModel>(filter, update));

            collection.BulkWrite(bulk);
        }

        private static void CheckUpdate(IMongoCollection<TestConcurrentModel> collection, int sum)
        {
            var filterSelect = Builders<TestConcurrentModel>.Filter.Where(d => d.LastUpdater != null);
            var documents = collection.Find(filterSelect).ToList();

            documents.Should()
                .NotBeEmpty()
                .And.NotContain(d => d.Counter != sum);
        }
    }
}