``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Job-CNGSRP : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

IterationCount=30  RunStrategy=Throughput  WarmupCount=3  

```
|                            Method |         Mean |        Error |       StdDev |
|---------------------------------- |-------------:|-------------:|-------------:|
|         MongoSelectUserByUserName |     843.2 us |     14.45 us |     21.18 us |
|      PostgresSelectUserByUserName |   3,136.4 us |    216.99 us |    311.20 us |
|     MongoSelectCommentsByUserName |  11,112.4 us |    756.00 us |  1,108.14 us |
|  PostgresSelectCommentsByUserName | 462,410.4 us | 17,378.27 us | 24,923.40 us |
| PostgresSelectCommentsByUserName2 |  79,333.7 us | 11,560.26 us | 17,302.86 us |
