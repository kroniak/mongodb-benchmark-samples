using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongodbTransactions.TestCases
{
    [CoreJob]
    public class MultiDocs
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private IEnumerable<BsonDocument> _documents;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        [Params(10, 50, 100)] 
        public int Count { get; set; }

        public MultiDocs()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("foo");
            _collection = _database.GetCollection<BsonDocument>("bar");
        }

        [GlobalSetup]
        public void CleanDb()
        {
            _database.DropCollection("bar");
            Console.WriteLine("Deleted Rows!!!");

            _documents = Enumerable.Range(0, Count).Select(i => new BsonDocument
            {
                {"counter", i},
                {"name", "MongoDB"},
                {"type", "Database"},
                {"count", 1},
                {
                    "info", new BsonDocument
                    {
                        {"x", 203},
                        {"y", 102}
                    }
                }
            });
            Console.WriteLine("Prepared Docs!!!");
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            CleanDb();
        }

        [Benchmark(Baseline = true)]
        public void Save()
        {
            _collection.InsertMany(_documents);
        }

        [Benchmark]
        public async Task SaveAsync()
        {
            await _collection.InsertManyAsync(_documents);
        }

        [Benchmark]
        public void SaveParallel()
        {
            Parallel.ForEach(_documents, document => { _collection.InsertOne(document); });
        }

        [Benchmark]
        public void SaveWithTransaction()
        {
            using (var session = _client.StartSession())
            {
                session.StartTransaction();
                Save();
                session.CommitTransaction();
            }
        }

        [Benchmark]
        public async Task SaveWithTransactionAsync()
        {
            using (var session = _client.StartSession())
            {
                session.StartTransaction();
                await SaveAsync();
                session.CommitTransaction();
            }
        }
    }
}