``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                   Method | Count |        Mean |     Error |      StdDev |      Median | Ratio | RatioSD |
|------------------------- |------ |------------:|----------:|------------:|------------:|------:|--------:|
|                     **Save** |    **10** |    **848.9 us** |  **16.92 us** |    **44.57 us** |    **838.9 us** |  **1.00** |    **0.00** |
|                 SaveBulk |    10 |    831.3 us |  16.40 us |    26.49 us |    830.8 us |  0.97 |    0.07 |
|                SaveAsync |    10 |    900.6 us |  17.57 us |    32.13 us |    894.7 us |  1.06 |    0.08 |
|             SaveParallel |    10 |  2,055.7 us |  41.79 us |   110.81 us |  2,035.2 us |  2.43 |    0.17 |
|      SaveWithTransaction |    10 |    835.1 us |  16.70 us |    32.57 us |    839.0 us |  0.99 |    0.08 |
|  SaveBulkWithTransaction |    10 |    846.7 us |  16.21 us |    15.93 us |    850.0 us |  1.00 |    0.06 |
| SaveWithTransactionAsync |    10 |    913.5 us |  18.11 us |    35.33 us |    907.2 us |  1.08 |    0.07 |
|                          |       |             |           |             |             |       |         |
|                     **Save** |    **50** |  **1,180.6 us** |  **23.37 us** |    **35.68 us** |  **1,182.6 us** |  **1.00** |    **0.00** |
|                 SaveBulk |    50 |  1,199.8 us |  23.87 us |    37.85 us |  1,201.7 us |  1.02 |    0.05 |
|                SaveAsync |    50 |  1,289.6 us |  25.61 us |    67.92 us |  1,277.5 us |  1.12 |    0.06 |
|             SaveParallel |    50 |  8,420.7 us | 189.05 us |   554.46 us |  8,408.1 us |  7.15 |    0.41 |
|      SaveWithTransaction |    50 |  1,212.2 us |  23.84 us |    41.75 us |  1,208.9 us |  1.02 |    0.04 |
|  SaveBulkWithTransaction |    50 |  1,196.7 us |  23.83 us |    57.09 us |  1,191.1 us |  1.01 |    0.06 |
| SaveWithTransactionAsync |    50 |  1,246.9 us |  24.81 us |    47.81 us |  1,249.3 us |  1.04 |    0.05 |
|                          |       |             |           |             |             |       |         |
|                     **Save** |   **100** |  **1,658.7 us** |  **34.28 us** |    **75.25 us** |  **1,638.5 us** |  **1.00** |    **0.00** |
|                 SaveBulk |   100 |  1,633.4 us |  32.60 us |    68.76 us |  1,620.4 us |  0.98 |    0.05 |
|                SaveAsync |   100 |  1,701.8 us |  33.62 us |    75.88 us |  1,687.8 us |  1.03 |    0.06 |
|             SaveParallel |   100 | 16,223.4 us | 438.45 us | 1,278.97 us | 16,146.0 us | 10.15 |    0.91 |
|      SaveWithTransaction |   100 |  1,804.3 us |  56.80 us |   166.58 us |  1,762.6 us |  1.08 |    0.12 |
|  SaveBulkWithTransaction |   100 |  1,658.1 us |  34.38 us |    88.13 us |  1,639.2 us |  1.01 |    0.07 |
| SaveWithTransactionAsync |   100 |  1,794.9 us |  52.99 us |   152.89 us |  1,743.0 us |  1.08 |    0.10 |
