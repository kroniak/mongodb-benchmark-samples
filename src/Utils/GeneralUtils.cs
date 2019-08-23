using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace MongodbTransactions.Utils
{
    public static class GeneralUtils
    {
        public const string PgConnectionString =
            "Host=docker;Username=postgres;Password=mysecretpassword;Database=postgres";

        public const string SqlConnectionString =
            "Server=docker;Database=master;User Id=sa;password='yourStrong(!)Password';MultipleActiveResultSets=true";
        
        public const string MongodbLocalhost = "mongodb://localhost:27017";

        public static IEnumerable<T> Select<T>(this IDataReader reader,
            Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
        
        public static void AddRange<T>(this ConcurrentBag<T> collection, IEnumerable<T> toAdd)
        {
            foreach (var element in toAdd)
            {
                collection.Add(element);
            }
        }
    }
}