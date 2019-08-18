using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using MongodbTransactions.CompareTestCases;
using MongodbTransactions.ConcurrentTestCases;
using MongodbTransactions.TestCases;

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

            Console.WriteLine("Job Done");
        }
    }
}