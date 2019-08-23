using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using MongodbTransactions.Blog;

namespace MongodbTransactions
{
    internal class Program
    {
        private static void Main()
        {
//            BenchmarkRunner.Run<MultiDocs>();
//            BenchmarkRunner.Run<MultiCollections>();
//            BenchmarkRunner.Run<MultiCollectionsUpdate>();
//            BenchmarkRunner.Run<CompareMultiDocs>();
//            BenchmarkRunner.Run<MultiCollectionsConcurrentUpdate>();
//            BenchmarkRunner.Run<SchemaValidator>();
//            BenchmarkRunner.Run<BlogNestedDocuments>(new DebugInProcessConfig());
//            BenchmarkRunner.Run<BlogNestedDocuments>();
//            BenchmarkRunner.Run<BlogSeparateDocuments>(new DebugInProcessConfig());
            BenchmarkRunner.Run<BlogSeparateDocuments>();


            Console.WriteLine("Job Done");
        }
    }
}