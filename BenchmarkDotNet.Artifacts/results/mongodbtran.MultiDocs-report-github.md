``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|              Method | Count |       Mean |    Error |   StdDev |     Median |
|-------------------- |------ |-----------:|---------:|---------:|-----------:|
|                **Save** |    **10** |   **648.3 us** | **29.85 us** | **84.20 us** |   **624.0 us** |
| SaveWithTransaction |    10 |   642.8 us | 17.13 us | 48.86 us |   634.4 us |
|                **Save** |    **50** |   **985.8 us** | **29.91 us** | **85.83 us** |   **968.1 us** |
| SaveWithTransaction |    50 |   998.1 us | 23.06 us | 65.41 us |   990.6 us |
|                **Save** |   **100** | **1,435.6 us** | **31.72 us** | **87.90 us** | **1,419.3 us** |
| SaveWithTransaction |   100 | 1,433.7 us | 28.65 us | 70.28 us | 1,415.6 us |
