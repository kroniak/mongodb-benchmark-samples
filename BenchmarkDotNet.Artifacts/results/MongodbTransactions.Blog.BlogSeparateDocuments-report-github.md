``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Job-MHRMYP : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

IterationCount=30  RunStrategy=Throughput  WarmupCount=3  

```
|                           Method |      Mean |     Error |    StdDev |
|--------------------------------- |----------:|----------:|----------:|
|    MongoSelectCommentsByUserName | 11.160 ms | 0.8713 ms | 1.2772 ms |
| PostgresSelectCommentsByUserName |  4.796 ms | 0.2024 ms | 0.2967 ms |
