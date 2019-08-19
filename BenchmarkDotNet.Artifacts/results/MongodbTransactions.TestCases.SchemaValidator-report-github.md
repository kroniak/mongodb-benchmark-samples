``` ini

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Xeon CPU E5-2676 v3 2.40GHz, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|             Method |    Mean |    Error |   StdDev | Ratio |
|------------------- |--------:|---------:|---------:|------:|
|               Save | 1.440 s | 0.0104 s | 0.0097 s |  1.00 |
| SaveWithValidation | 1.644 s | 0.0078 s | 0.0073 s |  1.14 |
