``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|             Method |    Mean |    Error |   StdDev | Ratio | RatioSD |
|------------------- |--------:|---------:|---------:|------:|--------:|
|               Save | 1.316 s | 0.0261 s | 0.0490 s |  1.00 |    0.00 |
| SaveWithValidation | 1.413 s | 0.0278 s | 0.0331 s |  1.08 |    0.05 |
