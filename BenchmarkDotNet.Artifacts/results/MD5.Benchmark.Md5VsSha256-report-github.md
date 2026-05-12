```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.26200.8246)
Unknown processor
.NET SDK 10.0.203
  [Host]     : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
  DefaultJob : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2


```
| Method | Mean     | Error    | StdDev   | Allocated |
|------- |---------:|---------:|---------:|----------:|
| Md5    | 14.77 μs | 0.170 μs | 0.159 μs |      80 B |
| StxMd5 | 22.27 μs | 0.204 μs | 0.191 μs |      40 B |
