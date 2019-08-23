using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using MongodbTransactions.Blog;
using MongodbTransactions.CompareTestCases;
using MongodbTransactions.ConcurrentTestCases;
using MongodbTransactions.TestCases;
using MongodbTransactions.NoSql;

namespace MongodbTransactions
{
    internal class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<MultiDocs>();
            BenchmarkRunner.Run<MultiCollections>();
            BenchmarkRunner.Run<MultiCollectionsUpdate>();
            BenchmarkRunner.Run<CompareMultiDocs>();
            BenchmarkRunner.Run<MultiCollectionsConcurrentUpdate>();
            BenchmarkRunner.Run<SchemaValidator>();
            BenchmarkRunner.Run<BlogNestedDocuments>();
            BenchmarkRunner.Run<BlogSeparateDocuments>();
            BenchmarkRunner.Run<NoSqlTest>();

            Console.WriteLine("Job Done");
        }
    }
}