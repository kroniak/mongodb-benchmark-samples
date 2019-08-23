using MongoDB.Bson.Serialization.Attributes;

namespace MongodbTransactions.NoSql.Models
{
    [BsonDiscriminator("AuthService")]
    public class AuthService : ServiceRecord
    {
        public bool IsAuth { get; set; }
        
        public string UserName { get; set; }
    }
}