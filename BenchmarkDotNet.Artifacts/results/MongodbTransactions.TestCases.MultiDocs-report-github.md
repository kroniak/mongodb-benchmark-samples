``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                   Method | Count |        Mean |     Error |      StdDev | Ratio | RatioSD |
|------------------------- |------ |------------:|----------:|------------:|------:|--------:|
|                     **Save** |    **10** |    **648.7 us** |  **26.73 us** |    **77.12 us** |  **1.00** |    **0.00** |
|                SaveAsync |    10 |    659.0 us |  13.65 us |    37.37 us |  1.03 |    0.12 |
|             SaveParallel |    10 |  1,950.8 us |  45.46 us |   129.70 us |  3.05 |    0.41 |
|      SaveWithTransaction |    10 |    581.7 us |  10.90 us |    23.00 us |  0.95 |    0.11 |
| SaveWithTransactionAsync |    10 |    652.4 us |  13.00 us |    34.92 us |  1.02 |    0.13 |
|                          |       |             |           |             |       |         |
|                     **Save** |    **50** |    **926.5 us** |  **18.45 us** |    **38.11 us** |  **1.00** |    **0.00** |
|                SaveAsync |    50 |    996.1 us |  19.76 us |    54.75 us |  1.07 |    0.08 |
|             SaveParallel |    50 |  8,955.6 us | 262.92 us |   771.09 us |  9.79 |    0.97 |
|      SaveWithTransaction |    50 |    915.5 us |  18.29 us |    36.11 us |  0.99 |    0.05 |
| SaveWithTransactionAsync |    50 |    977.0 us |  22.63 us |    52.89 us |  1.06 |    0.07 |
|                          |       |             |           |             |       |         |
|                     **Save** |   **100** |  **1,395.8 us** |  **27.68 us** |    **48.48 us** |  **1.00** |    **0.00** |
|                SaveAsync |   100 |  1,449.7 us |  27.71 us |    70.52 us |  1.03 |    0.07 |
|             SaveParallel |   100 | 17,953.5 us | 493.75 us | 1,455.85 us | 12.28 |    1.10 |
|      SaveWithTransaction |   100 |  1,430.4 us |  28.46 us |    41.72 us |  1.02 |    0.05 |
| SaveWithTransactionAsync |   100 |  1,441.3 us |  28.52 us |    66.67 us |  1.03 |    0.06 |
