using System;
using BenchmarkDotNet.Running;
using MongodbTransactions.TestCases;

namespace MongodbTransactions
{
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<MultiDocs>();
            BenchmarkRunner.Run<MultiCollections>();
            BenchmarkRunner.Run<MultiCollectionsUpdate>();

            Console.WriteLine("Job Done");
        }
    }
}