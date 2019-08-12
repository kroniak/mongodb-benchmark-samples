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
|                     SaveAndUpdate | 2.364 ms | 0.0469 ms | 0.0702 ms |  1.00 |    0.00 |
|                SaveAndUpdateAsync | 2.547 ms | 0.0507 ms | 0.1396 ms |  1.08 |    0.07 |
|      SaveAndUpdateWithTransaction | 2.445 ms | 0.0489 ms | 0.0732 ms |  1.04 |    0.04 |
| SaveAndUpdateWithTransactionAsync | 2.596 ms | 0.0511 ms | 0.0716 ms |  1.10 |    0.04 |
