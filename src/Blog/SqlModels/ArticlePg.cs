using System;
using System.Collections.Generic;

namespace MongodbTransactions.Blog.SqlModels
{
    public class ArticlePg
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime Created { get; set; }

        public string Text { get; set; }

        public long UserId { get; set; }

        public List<CommentPg> Comments { get; set; }
    }
}