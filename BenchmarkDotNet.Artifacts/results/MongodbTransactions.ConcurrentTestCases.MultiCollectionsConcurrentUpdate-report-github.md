``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Job-RSWWTZ : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

IterationCount=1  RunStrategy=ColdStart  

```
|                       Method |  Count | ThreadCount |     Mean | Error |
|----------------------------- |------- |------------ |---------:|------:|
| **SaveAndUpdateWithTransaction** | **100000** |           **5** | **170.7 ms** |    **NA** |
| **SaveAndUpdateWithTransaction** | **100000** |          **10** | **174.5 ms** |    **NA** |
| **SaveAndUpdateWithTransaction** | **100000** |          **20** | **281.7 ms** |    **NA** |
