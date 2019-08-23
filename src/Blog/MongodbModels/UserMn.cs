using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongodbTransactions.Blog.MongodbModels
{
    public class UserMn
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public string Url { get; set; }

        public string[] Articles { get; set; }
    }
}