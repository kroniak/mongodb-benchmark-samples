using System;
using MongoDB.Bson;

namespace MongodbTransactions.Blog.MongodbModels
{
    public class CommentMn
    {
        public DateTime Created { get; set; }

        public string Text { get; set; }

        public string UserId { get; set; }
    }
}