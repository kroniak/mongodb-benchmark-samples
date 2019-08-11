``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                            Method |     Mean |     Error |    StdDev | Ratio | RatioSD |
|---------------------------------- |---------:|----------:|----------:|------:|--------:|
|                     SaveAndUpdate | 2.076 ms | 0.0414 ms | 0.0968 ms |  1.00 |    0.00 |
|                SaveAndUpdateAsync | 2.305 ms | 0.0460 ms | 0.0830 ms |  1.13 |    0.07 |
|      SaveAndUpdateWithTransaction | 2.033 ms | 0.0405 ms | 0.0955 ms |  0.98 |    0.05 |
| SaveAndUpdateWithTransactionAsync | 2.139 ms | 0.0421 ms | 0.0433 ms |  1.01 |    0.04 |
