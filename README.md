# StxWright.MD5

![CI](https://github.com/stxwright/md5/actions/workflows/ci.yml/badge.svg?branch=master)

A pure C# MD5 implementation optimised for small inputs. Outperforms the BCL implementation for inputs under ~512 bytes, with half the allocations at all sizes.

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

| Method    | Size    | Mean           | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------- |-------- |---------------:|----------:|----------:|------:|-------:|----------:|------------:|
| **SystemMd5** | **64**      |       **696.8 ns** |   **1.93 ns** |   **1.50 ns** |  **1.00** | **0.0048** |      **80 B** |        **1.00** |
| StxMd5    | 64      |       386.5 ns |   0.18 ns |   0.15 ns |  0.55 | 0.0024 |      40 B |        0.50 |
|           |         |                |           |           |       |        |           |             |
| **SystemMd5** | **1024**    |     **2,328.2 ns** |   **0.70 ns** |   **0.62 ns** |  **1.00** | **0.0038** |      **80 B** |        **1.00** |
| StxMd5    | 1024    |     2,879.6 ns |   1.20 ns |   1.00 ns |  1.24 |      - |      40 B |        0.50 |
|           |         |                |           |           |       |        |           |             |
| **SystemMd5** | **1048576** | **1,760,344.6 ns** | **194.44 ns** | **162.36 ns** |  **1.00** |      **-** |      **81 B** |        **1.00** |
| StxMd5    | 1048576 | 2,714,063.4 ns | 737.22 ns | 615.61 ns |  1.54 |      - |      43 B |        0.53 |

<!-- BENCHMARK_END -->

## License

MIT
