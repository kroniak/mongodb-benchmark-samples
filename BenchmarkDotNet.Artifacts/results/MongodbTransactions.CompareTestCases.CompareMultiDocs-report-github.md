``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                      Method | Count |        Mean |       Error |      StdDev | Ratio | RatioSD |
|---------------------------- |------ |------------:|------------:|------------:|------:|--------:|
|                **SavePostgres** |    **10** |  **8,595.7 us** |   **168.87 us** |   **236.73 us** | **13.73** |    **0.42** |
| SavePostgresWithTransaction |    10 |  6,932.9 us |   135.95 us |   215.63 us | 10.88 |    0.43 |
|                   SaveMongo |    10 |    634.5 us |    12.12 us |    12.45 us |  1.00 |    0.00 |
|         SaveWithTransaction |    10 |    666.8 us |    16.40 us |    21.90 us |  1.05 |    0.04 |
|                             |       |             |             |             |       |         |
|                **SavePostgres** |    **50** | **41,577.5 us** |   **823.22 us** | **2,110.23 us** | **43.57** |    **2.99** |
| SavePostgresWithTransaction |    50 | 29,399.7 us |   583.40 us | 1,217.77 us | 30.89 |    1.68 |
|                   SaveMongo |    50 |    953.0 us |    18.79 us |    39.63 us |  1.00 |    0.00 |
|         SaveWithTransaction |    50 |    959.9 us |    19.06 us |    20.39 us |  1.02 |    0.04 |
|                             |       |             |             |             |       |         |
|                **SavePostgres** |   **100** | **83,339.0 us** | **1,665.07 us** | **3,548.40 us** | **60.83** |    **4.03** |
| SavePostgresWithTransaction |   100 | 59,496.1 us | 1,183.82 us | 3,319.56 us | 44.35 |    2.87 |
|                   SaveMongo |   100 |  1,372.6 us |    29.77 us |    65.96 us |  1.00 |    0.00 |
|         SaveWithTransaction |   100 |  1,358.6 us |    26.82 us |    57.15 us |  0.99 |    0.05 |
