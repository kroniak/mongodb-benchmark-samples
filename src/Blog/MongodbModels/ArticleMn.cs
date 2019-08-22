using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongodbTransactions.Blog.MongodbModels
{
    public class ArticleMn
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime Created { get; set; }

        public string Text { get; set; }

        public List<CommentMn> Comments { get; set; }
    }
}