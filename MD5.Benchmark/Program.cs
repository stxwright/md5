using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace MD5.Benchmark
{
    [MemoryDiagnoser]
    public class Md5Comparison
    {
        [Params(64, 1024, 1048576)]
        public int Size { get; set; }

        private byte[] data = null!;
        private readonly System.Security.Cryptography.MD5 systemMd5 = System.Security.Cryptography.MD5.Create();
        private readonly StxWright.MD5 stxMd5 = StxWright.MD5.Create();

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[Size];
            new Random(42).NextBytes(data);
        }

        [Benchmark(Baseline = true)]
        public byte[] SystemMd5() => systemMd5.ComputeHash(data);

        [Benchmark]
        public byte[] StxMd5() => stxMd5.ComputeHash(data);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}