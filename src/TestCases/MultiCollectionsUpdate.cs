using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongodbTransactions.TestCases
{
    [CoreJob]
    public class MultiCollectionsUpdate
    {
        private IMongoCollection<BsonDocument> _collectionBar;
        private IMongoCollection<BsonDocument> _collectionBaz;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public MultiCollectionsUpdate()
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

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            CleanDb();
        }

        [Benchmark(Baseline = true)]
        public void SaveAndUpdate()
        {
            var documentBar = new BsonDocument
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
            var documentBaz = new BsonDocument
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
            _collectionBar.InsertOne(documentBar);
            _collectionBaz.InsertOne(documentBaz);

            var filter = new BsonDocument("_id", documentBar["_id"]);
            var update = new BsonDocument("$set", new BsonDocument("count", (int) documentBar["count"] - 1));
            _collectionBar.UpdateOne(filter, update);

            var filter2 = new BsonDocument("_id", documentBaz["_id"]);
            var update2 = new BsonDocument("$set", new BsonDocument("count", (int) documentBaz["count"] - 1));
            _collectionBaz.UpdateOne(filter2, update2);
        }

        [Benchmark]
        public async Task SaveAndUpdateAsync()
        {
            var documentBar = new BsonDocument
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
            var documentBaz = new BsonDocument
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
            await _collectionBar.InsertOneAsync(documentBar);
            await _collectionBaz.InsertOneAsync(documentBaz);

            var filter = new BsonDocument("_id", documentBar["_id"]);
            var updateData = new BsonDocument("$set", new BsonDocument("count", (int) documentBar["count"] - 1));

            var filter2 = new BsonDocument("_id", documentBaz["_id"]);
            var updateData2 = new BsonDocument("$set", new BsonDocument("count", (int) documentBaz["count"] - 1));

            await _collectionBar.UpdateOneAsync(filter, updateData);
            await _collectionBaz.UpdateOneAsync(filter2, updateData2);
        }

        [Benchmark]
        public void SaveAndUpdateWithTransaction()
        {
            using (var session = _client.StartSession())
            {
                session.StartTransaction();
                SaveAndUpdate();
                session.CommitTransaction();
            }
        }

        [Benchmark]
        public async Task SaveAndUpdateWithTransactionAsync()
        {
            using (var session = _client.StartSession())
            {
                session.StartTransaction();
                await SaveAndUpdateAsync();
                session.CommitTransaction();
            }
        }
    }
}