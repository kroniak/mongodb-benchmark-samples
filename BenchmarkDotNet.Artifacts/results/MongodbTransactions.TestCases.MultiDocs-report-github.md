``` ini

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Xeon CPU E5-2676 v3 2.40GHz, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                   Method | Count |        Mean |      Error |     StdDev | Ratio | RatioSD |
|------------------------- |------ |------------:|-----------:|-----------:|------:|--------:|
|                     **Save** |    **10** |    **424.8 us** |   **4.511 us** |   **4.220 us** |  **1.00** |    **0.00** |
|                 SaveBulk |    10 |    422.7 us |   2.783 us |   2.467 us |  1.00 |    0.01 |
|                SaveAsync |    10 |    538.9 us |   3.858 us |   3.420 us |  1.27 |    0.01 |
|             SaveParallel |    10 |  1,314.1 us |  26.046 us |  34.771 us |  3.12 |    0.10 |
|      SaveWithTransaction |    10 |    429.2 us |   5.614 us |   5.251 us |  1.01 |    0.02 |
|  SaveBulkWithTransaction |    10 |    425.8 us |   4.487 us |   4.197 us |  1.00 |    0.01 |
| SaveWithTransactionAsync |    10 |    546.7 us |   2.557 us |   2.392 us |  1.29 |    0.02 |
|                          |       |             |            |            |       |         |
|                     **Save** |    **50** |    **920.5 us** |   **3.942 us** |   **3.688 us** |  **1.00** |    **0.00** |
|                 SaveBulk |    50 |    905.0 us |   4.562 us |   4.267 us |  0.98 |    0.01 |
|                SaveAsync |    50 |  1,020.4 us |  12.728 us |  11.906 us |  1.11 |    0.01 |
|             SaveParallel |    50 |  5,965.1 us | 108.597 us | 101.582 us |  6.48 |    0.11 |
|      SaveWithTransaction |    50 |    928.0 us |   2.596 us |   2.302 us |  1.01 |    0.00 |
|  SaveBulkWithTransaction |    50 |    918.2 us |   8.089 us |   7.171 us |  1.00 |    0.01 |
| SaveWithTransactionAsync |    50 |  1,033.8 us |  10.177 us |   9.520 us |  1.12 |    0.01 |
|                          |       |             |            |            |       |         |
|                     **Save** |   **100** |  **1,523.0 us** |   **8.584 us** |   **8.029 us** |  **1.00** |    **0.00** |
|                 SaveBulk |   100 |  1,519.0 us |   8.499 us |   7.950 us |  1.00 |    0.01 |
|                SaveAsync |   100 |  1,614.2 us |  10.112 us |   9.458 us |  1.06 |    0.01 |
|             SaveParallel |   100 | 11,464.5 us | 180.740 us | 160.221 us |  7.53 |    0.09 |
|      SaveWithTransaction |   100 |  1,502.9 us |   4.588 us |   4.291 us |  0.99 |    0.01 |
|  SaveBulkWithTransaction |   100 |  1,486.1 us |   4.387 us |   3.889 us |  0.98 |    0.01 |
| SaveWithTransactionAsync |   100 |  1,624.5 us |  10.118 us |   9.464 us |  1.07 |    0.01 |
