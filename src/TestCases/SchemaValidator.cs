using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongodbTransactions.TestCases
{
    [CoreJob]
    public class SchemaValidator
    {
        private IMongoCollection<BsonDocument> _collectionBarValidate;
        private IMongoCollection<BsonDocument> _collectionBar;
        private readonly IMongoDatabase _database;
        private IEnumerable<InsertOneModel<BsonDocument>> _bulk;

        public SchemaValidator()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("foo");
        }

        [GlobalSetup]
        public void CleanDb()
        {
            _database.DropCollection("bar_validate");
            _database.DropCollection("bar");

            var requiredFields = new[] {"name", "counter", "type", "info", "count"};
            _database.CreateCollection("bar_validate", new CreateCollectionOptions<BsonDocument>
            {
                Validator = new BsonDocument
                {
                    {
                        "$jsonSchema", new BsonDocument
                        {
                            {"required", new BsonArray(requiredFields)},
                            {
                                "properties", new BsonDocument
                                {
                                    {
                                        "count", new BsonDocument
                                        {
                                            {"bsonType", "int"}
                                        }
                                    },
                                    {
                                        "info", new BsonDocument
                                        {
                                            {"bsonType", "object"},
                                            {
                                                "properties", new BsonDocument
                                                {
                                                    {
                                                        "x", new BsonDocument
                                                        {
                                                            {"bsonType", "int"},
                                                            {"maximum", 256}
                                                        }
                                                    },
                                                    {
                                                        "y", new BsonDocument
                                                        {
                                                            {"bsonType", "int"},
                                                            {"maximum", 256}
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
            _collectionBarValidate = _database.GetCollection<BsonDocument>("bar_validate");
            _collectionBar = _database.GetCollection<BsonDocument>("bar");
            Console.WriteLine("Deleted Rows!!!");
            _bulk = Enumerable.Range(0, 100000).Select(i =>
                new InsertOneModel<BsonDocument>(new BsonDocument
                {
                    {
                        "counter", i
                    },
                    {
                        "name", "MongoDB"
                    },
                    {
                        "type", "Database"
                    },
                    {
                        "count", 1
                    },
                    {
                        "info", new BsonDocument
                        {
                            {"x", 203},
                            {"y", 102}
                        }
                    }
                }));

            Console.WriteLine("Prepared Docs!!!");
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
//            CleanDb();
        }

        [
            Benchmark(Baseline = true)]
        public void Save()
        {
            _collectionBar.BulkWrite(_bulk);
        }

        [
            Benchmark]
        public void SaveWithValidation()
        {
            _collectionBarValidate.BulkWrite(_bulk);
        }
    }
}