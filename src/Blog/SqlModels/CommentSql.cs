using System;

namespace MongodbTransactions.Blog.SqlModels
{
    public class CommentSql
    {
        public long Id { get; set; }
            
        public DateTime Created { get; set; }

        public string Text { get; set; }

        public long UserId { get; set; }
        
        public long ArticleId { get; set; }
    }
}