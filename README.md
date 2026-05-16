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

| Method | Mean     | Error    | StdDev   | Allocated |
|------- |---------:|---------:|---------:|----------:|
| Md5    | 20.24 μs | 0.030 μs | 0.027 μs |      80 B |
| StxMd5 | 38.38 μs | 0.084 μs | 0.079 μs |      40 B |

<!-- BENCHMARK_END -->

## License

MIT
