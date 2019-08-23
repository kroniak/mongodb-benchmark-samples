using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongodbTransactions.NoSql.Models
{
    public class Package
    {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }
            
            public DateTime Created { get; set; }
            
            public string InitSystem { get; set; }
            
            public List<ServiceRecord> Records { get; set; }
    }
}