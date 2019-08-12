``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                   Method | Count |        Mean |     Error |    StdDev |      Median | Ratio | RatioSD |
|------------------------- |------ |------------:|----------:|----------:|------------:|------:|--------:|
|                     **Save** |    **10** |    **640.1 us** |  **12.65 us** |  **29.07 us** |    **638.9 us** |  **1.00** |    **0.00** |
|                SaveAsync |    10 |    704.7 us |  14.76 us |  22.98 us |    700.7 us |  1.11 |    0.06 |
|             SaveParallel |    10 |  1,693.4 us |  36.31 us | 101.83 us |  1,684.0 us |  2.68 |    0.25 |
|      SaveWithTransaction |    10 |    632.6 us |  12.55 us |  24.18 us |    636.8 us |  1.00 |    0.06 |
| SaveWithTransactionAsync |    10 |    709.0 us |  13.81 us |  22.30 us |    714.6 us |  1.12 |    0.06 |
|                          |       |             |           |           |             |       |         |
|                     **Save** |    **50** |    **983.0 us** |  **18.66 us** |  **17.45 us** |    **980.5 us** |  **1.00** |    **0.00** |
|                SaveAsync |    50 |  1,089.6 us |  22.69 us |  53.49 us |  1,080.4 us |  1.10 |    0.05 |
|             SaveParallel |    50 |  7,069.2 us | 176.35 us | 517.21 us |  6,922.0 us |  6.80 |    0.24 |
|      SaveWithTransaction |    50 |  1,002.8 us |  19.97 us |  37.01 us |    991.4 us |  1.03 |    0.04 |
| SaveWithTransactionAsync |    50 |  1,073.8 us |  23.29 us |  66.83 us |  1,058.1 us |  1.05 |    0.05 |
|                          |       |             |           |           |             |       |         |
|                     **Save** |   **100** |  **1,412.5 us** |  **19.42 us** |  **16.21 us** |  **1,408.4 us** |  **1.00** |    **0.00** |
|                SaveAsync |   100 |  1,543.9 us |  30.85 us |  54.84 us |  1,539.8 us |  1.09 |    0.03 |
|             SaveParallel |   100 | 13,361.2 us | 266.93 us | 679.42 us | 13,348.1 us |  9.25 |    0.60 |
|      SaveWithTransaction |   100 |  1,483.2 us |  31.65 us |  46.39 us |  1,479.8 us |  1.05 |    0.03 |
| SaveWithTransactionAsync |   100 |  1,572.4 us |  31.11 us |  67.64 us |  1,560.3 us |  1.09 |    0.04 |
