using MongoDB.Bson.Serialization.Attributes;

namespace MongodbTransactions.NoSql.Models
{
    [BsonDiscriminator("BillingService")]
    public class BillingService : ServiceRecord
    {
        public bool IsBilled { get; set; }
        
        public string Provider { get; set; }
    }
}