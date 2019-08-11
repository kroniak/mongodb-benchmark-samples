``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G87) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host] : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  Core   : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                       Method |     Mean |     Error |    StdDev |   Median |
|----------------------------- |---------:|----------:|----------:|---------:|
|                SaveAndUpdate | 2.154 ms | 0.0423 ms | 0.0805 ms | 2.132 ms |
| SaveAndUpdateWithTransaction | 2.267 ms | 0.0556 ms | 0.1550 ms | 2.200 ms |
