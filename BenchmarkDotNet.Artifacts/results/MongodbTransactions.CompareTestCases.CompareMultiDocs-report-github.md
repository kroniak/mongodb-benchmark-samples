``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                      Method | Count |        Mean |        Error |       StdDev |      Median | Ratio | RatioSD |
|---------------------------- |------ |------------:|-------------:|-------------:|------------:|------:|--------:|
|                **SavePostgres** |    **10** |  **7,181.3 us** |   **168.838 us** |   **478.965 us** |  **7,179.3 us** | **12.10** |    **1.20** |
| SavePostgresWithTransaction |    10 |  6,435.8 us |   152.117 us |   134.848 us |  6,413.1 us | 11.34 |    0.72 |
|                   SaveMongo |    10 |    587.7 us |    11.597 us |    25.939 us |    592.2 us |  1.00 |    0.00 |
|         SaveWithTransaction |    10 |    546.5 us |     7.751 us |     6.871 us |    545.7 us |  0.96 |    0.06 |
|                             |       |             |              |              |             |       |         |
|                **SavePostgres** |    **50** | **34,285.2 us** |   **748.609 us** | **2,207.291 us** | **34,310.1 us** | **36.76** |    **3.65** |
| SavePostgresWithTransaction |    50 | 26,648.1 us |   481.336 us |   426.692 us | 26,556.7 us | 29.63 |    1.97 |
|                   SaveMongo |    50 |    941.4 us |    28.305 us |    80.757 us |    932.1 us |  1.00 |    0.00 |
|         SaveWithTransaction |    50 |    885.4 us |    18.189 us |    45.633 us |    879.9 us |  0.94 |    0.10 |
|                             |       |             |              |              |             |       |         |
|                **SavePostgres** |   **100** | **67,434.4 us** | **1,225.741 us** | **1,023.549 us** | **67,195.8 us** | **51.84** |    **2.93** |
| SavePostgresWithTransaction |   100 | 48,447.9 us |   966.379 us | 2,258.879 us | 48,915.4 us | 37.50 |    3.12 |
|                   SaveMongo |   100 |  1,294.6 us |    27.394 us |    77.713 us |  1,266.9 us |  1.00 |    0.00 |
|         SaveWithTransaction |   100 |  1,305.3 us |    26.987 us |    66.705 us |  1,293.1 us |  1.01 |    0.08 |
