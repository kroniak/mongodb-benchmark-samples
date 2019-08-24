``` ini

BenchmarkDotNet=v0.11.5, OS=Ubuntu 18.04
Intel Xeon-5350 CPU 2.40GHz, 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Job-JDNROT : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

IterationCount=30  RunStrategy=Throughput  WarmupCount=3  

```
|                             Method |      Mean |     Error |    StdDev |
|----------------------------------- |----------:|----------:|----------:|
|    MongoSelectAuthAndBilledRecords |  43.67 ms |  2.113 ms |  3.435 ms |
| PostgresSelectAuthAndBilledRecords |  65.12 ms |  6.453 ms | 15.657 ms |
|      SqlSelectAuthAndBilledRecords |  41.40 ms |  1.396 ms |  2.945 ms |