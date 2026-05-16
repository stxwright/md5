# StxWright.MD5

![CI](https://github.com/stxwright/md5/actions/workflows/ci.yml/badge.svg?branch=master)

A high-performance MD5 implementation in C#.

## Usage

```csharp
using (var md5 = StxWright.MD5.Create())
{
    byte[] hashBytes = md5.ComputeHash(inputBytes);
    string hexHash = Convert.ToHexString(hashBytes);
}
```

## Running Tests

```bash
dotnet test
```

## Benchmarks

<!-- BENCHMARK_START -->

| Method    | Size    | Mean           | Error        | StdDev       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------- |-------- |---------------:|-------------:|-------------:|------:|--------:|-------:|----------:|------------:|
| **SystemMd5** | **64**      |       **483.1 ns** |      **8.98 ns** |      **7.96 ns** |  **1.00** |    **0.00** | **0.0095** |      **80 B** |        **1.00** |
| StxMd5    | 64      |       215.1 ns |      2.01 ns |      1.78 ns |  0.45 |    0.01 | 0.0048 |      40 B |        0.50 |
|           |         |                |              |              |       |         |        |           |             |
| **SystemMd5** | **1024**    |     **1,414.6 ns** |     **28.20 ns** |     **43.07 ns** |  **1.00** |    **0.00** | **0.0095** |      **80 B** |        **1.00** |
| StxMd5    | 1024    |     1,663.0 ns |     33.22 ns |     59.90 ns |  1.17 |    0.02 | 0.0038 |      40 B |        0.50 |
|           |         |                |              |              |       |         |        |           |             |
| **SystemMd5** | **1048576** | **1,041,183.2 ns** |  **1,776.61 ns** |  **1,483.55 ns** |  **1.00** |    **0.00** |      **-** |      **81 B** |        **1.00** |
| StxMd5    | 1048576 | 1,732,414.4 ns | 32,084.37 ns | 31,511.15 ns |  1.66 |    0.03 |      - |      41 B |        0.51 |

<!-- BENCHMARK_END -->

## License

MIT
