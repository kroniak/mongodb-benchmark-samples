``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                   Method |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD |
|------------------------- |---------:|----------:|----------:|---------:|------:|--------:|
|                     Save | 1.267 ms | 0.1005 ms | 0.2819 ms | 1.129 ms |  1.00 |    0.00 |
|                SaveAsync | 1.313 ms | 0.0511 ms | 0.1492 ms | 1.258 ms |  1.08 |    0.24 |
|      SaveWithTransaction | 1.147 ms | 0.0556 ms | 0.1630 ms | 1.133 ms |  0.94 |    0.23 |
| SaveWithTransactionAsync | 1.231 ms | 0.0455 ms | 0.1341 ms | 1.173 ms |  1.01 |    0.24 |
