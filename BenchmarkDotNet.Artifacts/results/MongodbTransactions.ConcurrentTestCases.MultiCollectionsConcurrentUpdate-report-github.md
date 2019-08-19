``` ini

BenchmarkDotNet=v0.11.5, OS=ubuntu 18.04
Intel Xeon CPU E5-2676 v3 2.40GHz, 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Job-GGFJXW : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

IterationCount=1  RunStrategy=ColdStart  

```
|                       Method |  Count | ThreadCount |     Mean | Error |
|----------------------------- |------- |------------ |---------:|------:|
| **SaveAndUpdateWithTransaction** | **100000** |           **5** | **224.7 ms** |    **NA** |
| **SaveAndUpdateWithTransaction** | **100000** |          **10** | **268.2 ms** |    **NA** |
| **SaveAndUpdateWithTransaction** | **100000** |          **20** | **367.7 ms** |    **NA** |
