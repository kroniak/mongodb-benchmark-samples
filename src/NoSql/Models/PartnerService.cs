using MongoDB.Bson.Serialization.Attributes;

namespace MongodbTransactions.NoSql.Models
{
    [BsonDiscriminator("PartnerService")]
    public class PartnerService : ServiceRecord
    {
        public bool IsDone { get; set; }
        
        public string Partner { get; set; }
    }
}