``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                   Method |     Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------- |---------:|----------:|----------:|------:|--------:|
|                     Save | 1.135 ms | 0.0226 ms | 0.0631 ms |  1.00 |    0.00 |
|                SaveAsync | 1.231 ms | 0.0246 ms | 0.0241 ms |  1.08 |    0.04 |
|      SaveWithTransaction | 1.155 ms | 0.0232 ms | 0.0638 ms |  1.02 |    0.07 |
| SaveWithTransactionAsync | 1.268 ms | 0.0251 ms | 0.0530 ms |  1.14 |    0.07 |
