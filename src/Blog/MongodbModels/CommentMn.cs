using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongodbTransactions.Blog.MongodbModels
{
    public class CommentMn
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public DateTime Created { get; set; }

        public string Text { get; set; }

        public string ArticleId { get; set; }
        
        public string UserId { get; set; }
    }
}