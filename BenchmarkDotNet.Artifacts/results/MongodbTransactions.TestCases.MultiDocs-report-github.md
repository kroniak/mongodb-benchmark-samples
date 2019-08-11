``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|              Method | Count |       Mean |    Error |    StdDev |     Median |
|-------------------- |------ |-----------:|---------:|----------:|-----------:|
|                **Save** |    **10** |   **673.9 us** | **30.71 us** |  **87.61 us** |   **667.2 us** |
| SaveWithTransaction |    10 |   597.2 us | 12.23 us |  30.91 us |   584.9 us |
|                **Save** |    **50** |   **966.1 us** | **19.30 us** |  **32.24 us** |   **968.2 us** |
| SaveWithTransaction |    50 | 1,008.0 us | 24.48 us |  71.41 us |   987.0 us |
|                **Save** |   **100** | **1,543.4 us** | **49.86 us** | **143.07 us** | **1,493.9 us** |
| SaveWithTransaction |   100 | 1,679.6 us | 86.37 us | 243.60 us | 1,643.2 us |
