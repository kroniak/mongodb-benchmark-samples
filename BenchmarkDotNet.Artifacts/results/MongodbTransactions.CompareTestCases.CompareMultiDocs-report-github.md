``` ini

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Xeon CPU E5-2676 v3 2.40GHz, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                      Method | Count |        Mean |        Error |       StdDev | Ratio | RatioSD |
|---------------------------- |------ |------------:|-------------:|-------------:|------:|--------:|
|                **SavePostgres** |    **10** |  **9,622.8 us** |   **184.733 us** |   **205.330 us** | **20.54** |    **0.46** |
| SavePostgresWithTransaction |    10 |  3,712.4 us |    31.424 us |    27.857 us |  7.94 |    0.09 |
|                   SaveMongo |    10 |    467.0 us |     3.211 us |     3.004 us |  1.00 |    0.00 |
|    SaveMongoWithTransaction |    10 |    478.0 us |     4.979 us |     4.414 us |  1.02 |    0.01 |
|                             |       |             |              |              |       |         |
|                **SavePostgres** |    **50** | **46,394.8 us** |   **882.909 us** |   **906.682 us** | **53.36** |    **1.22** |
| SavePostgresWithTransaction |    50 | 15,043.0 us |   100.412 us |    93.925 us | 17.27 |    0.18 |
|                   SaveMongo |    50 |    870.9 us |     6.709 us |     6.275 us |  1.00 |    0.00 |
|    SaveMongoWithTransaction |    50 |    871.8 us |     3.738 us |     3.314 us |  1.00 |    0.01 |
|                             |       |             |              |              |       |         |
|                **SavePostgres** |   **100** | **93,013.9 us** | **1,828.335 us** | **1,710.226 us** | **71.23** |    **1.34** |
| SavePostgresWithTransaction |   100 | 28,892.4 us |   320.876 us |   284.448 us | 22.14 |    0.32 |
|                   SaveMongo |   100 |  1,305.8 us |    12.626 us |    11.810 us |  1.00 |    0.00 |
|    SaveMongoWithTransaction |   100 |  1,314.4 us |     5.977 us |     5.591 us |  1.01 |    0.01 |
