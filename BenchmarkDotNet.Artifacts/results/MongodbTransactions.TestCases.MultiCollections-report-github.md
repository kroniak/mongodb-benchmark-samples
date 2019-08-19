``` ini

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Xeon CPU E5-2676 v3 2.40GHz, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                   Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|------------------------- |---------:|---------:|---------:|------:|--------:|
|                     Save | 642.4 us | 7.182 us | 6.367 us |  1.00 |    0.00 |
|                SaveAsync | 834.9 us | 4.198 us | 3.927 us |  1.30 |    0.01 |
|      SaveWithTransaction | 649.5 us | 9.638 us | 9.015 us |  1.01 |    0.02 |
| SaveWithTransactionAsync | 842.3 us | 4.697 us | 4.164 us |  1.31 |    0.01 |
