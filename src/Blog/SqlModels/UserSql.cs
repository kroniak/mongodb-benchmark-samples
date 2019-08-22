using System;

namespace MongodbTransactions.Blog.SqlModels
{
    public class UserSql
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public string Url { get; set; }
    }
}