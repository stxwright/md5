using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace MD5.Benchmark
{
    [MemoryDiagnoser]
    public class Md5VsSha256
    {
        private const int N = 10000;
        private readonly byte[] data;

        private readonly System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        private readonly StxWright.MD5 stxMd5 = StxWright.MD5.Create();

        public Md5VsSha256()
        {
            data = new byte[N];
            new Random(42).NextBytes(data);
        }

/*        [Benchmark]
        public byte[] Md5() => md5.ComputeHash(data);*/


        [Benchmark]
        public byte[] StxMd5() => stxMd5.ComputeHash(data);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Md5VsSha256>();
        }
    }
}