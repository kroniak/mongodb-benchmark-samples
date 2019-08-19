``` ini

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Xeon CPU E5-2676 v3 2.40GHz, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                            Method |     Mean |     Error |    StdDev | Ratio | RatioSD |
|---------------------------------- |---------:|----------:|----------:|------:|--------:|
|                     SaveAndUpdate | 1.369 ms | 0.0237 ms | 0.0222 ms |  1.00 |    0.00 |
|                SaveAndUpdateAsync | 1.751 ms | 0.0072 ms | 0.0067 ms |  1.28 |    0.02 |
|      SaveAndUpdateWithTransaction | 1.416 ms | 0.0231 ms | 0.0205 ms |  1.03 |    0.02 |
| SaveAndUpdateWithTransactionAsync | 1.746 ms | 0.0093 ms | 0.0082 ms |  1.28 |    0.02 |
