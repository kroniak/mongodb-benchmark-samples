using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MongodbTransactions.NoSql.Models
{
    [BsonKnownTypes(typeof(AuthService), typeof(BillingService), typeof(PartnerService))]
    public class ServiceRecord
    {
        public string Name { get; set; }

        public DateTime Created { get; set; }
    }
}