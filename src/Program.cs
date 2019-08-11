using System;
using BenchmarkDotNet.Running;
using MongodbTransactions.CompareTestCases;
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

            Console.WriteLine("Job Done");
        }
    }
}