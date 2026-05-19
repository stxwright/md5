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

| Method    | Size | Mean       | Error   | StdDev  | Ratio | Gen0   | Allocated | Alloc Ratio |
|---------- |----- |-----------:|--------:|--------:|------:|-------:|----------:|------------:|
| **SystemMd5** | **64**   | **1,096.5 ns** | **4.15 ns** | **3.68 ns** |  **1.00** | **0.0038** |      **80 B** |        **1.00** |
| StxMd5    | 64   |   367.3 ns | 0.74 ns | 0.58 ns |  0.33 | 0.0024 |      40 B |        0.50 |
|           |      |            |         |         |       |        |           |             |
| **SystemMd5** | **128**  |   **811.4 ns** | **2.16 ns** | **1.91 ns** |  **1.00** | **0.0048** |      **80 B** |        **1.00** |
| StxMd5    | 128  |   529.3 ns | 0.24 ns | 0.19 ns |  0.65 | 0.0019 |      40 B |        0.50 |
|           |      |            |         |         |       |        |           |             |
| **SystemMd5** | **256**  | **1,034.5 ns** | **0.65 ns** | **0.54 ns** |  **1.00** | **0.0038** |      **80 B** |        **1.00** |
| StxMd5    | 256  |   869.5 ns | 1.02 ns | 0.85 ns |  0.84 | 0.0019 |      40 B |        0.50 |
|           |      |            |         |         |       |        |           |             |
| **SystemMd5** | **512**  | **1,458.3 ns** | **4.27 ns** | **3.57 ns** |  **1.00** | **0.0038** |      **80 B** |        **1.00** |
| StxMd5    | 512  | 1,514.6 ns | 0.66 ns | 0.51 ns |  1.04 | 0.0019 |      40 B |        0.50 |
|           |      |            |         |         |       |        |           |             |
| **SystemMd5** | **1024** | **2,312.3 ns** | **1.90 ns** | **1.69 ns** |  **1.00** | **0.0038** |      **80 B** |        **1.00** |
| StxMd5    | 1024 | 2,855.4 ns | 0.48 ns | 0.40 ns |  1.23 |      - |      40 B |        0.50 |

<!-- BENCHMARK_END -->

## License

MIT
