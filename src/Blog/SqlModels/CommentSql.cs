using System;

namespace MongodbTransactions.Blog.SqlModels
{
    public class CommentSql
    {
        public DateTime Created { get; set; }

        public string Text { get; set; }

        public long UserId { get; set; }
    }
}