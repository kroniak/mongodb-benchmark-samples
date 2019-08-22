``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Job-BOGFWR : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

IterationCount=30  RunStrategy=Throughput  WarmupCount=3  

```
|                            Method |         Mean |        Error |       StdDev |
|---------------------------------- |-------------:|-------------:|-------------:|
|         MongoSelectUserByUserName |     800.3 us |     26.22 us |     39.25 us |
|      PostgresSelectUserByUserName |   3,208.3 us |    263.37 us |    394.20 us |
|     MongoSelectCommentsByUserName |  10,013.4 us |    649.40 us |    971.99 us |
|  PostgresSelectCommentsByUserName | 454,513.7 us | 12,459.90 us | 17,467.04 us |
| PostgresSelectCommentsByUserName2 |  83,330.2 us |  4,219.16 us |  6,315.04 us |
