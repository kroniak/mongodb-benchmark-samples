using System;
using BenchmarkDotNet.Running;

namespace mongodbtran
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MultiDocs>();
            BenchmarkRunner.Run<MultiCollections>();

            Console.WriteLine("Job Done");
        }
    }
}