using System;
using BenchmarkDotNet.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongodbTransactions.TestCases
{
    [CoreJob]
    public class MultiCollections
    {
        private IMongoCollection<BsonDocument> _collectionBar;
        private IMongoCollection<BsonDocument> _collectionBaz;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public MultiCollections()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("foo");
        }

        [GlobalSetup]
        public void CleanDb()
        {
            _database.DropCollection("bar");
            _database.DropCollection("baz");
            _collectionBar = _database.GetCollection<BsonDocument>("bar");
            _collectionBaz = _database.GetCollection<BsonDocument>("baz");

            Console.WriteLine("Deleted Rows!!!");
        }

        [Benchmark]
        public void Save()
        {
            var document = new BsonDocument
            {
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
            };
            _collectionBar.InsertOne(document);
            _collectionBaz.InsertOne(document);
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
    }
}