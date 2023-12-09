using NUnit.Framework;
using System;
using System.Text;

namespace MD5.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("The quick brown fox jumps over the lazy dog", "9e107d9d372bb6826bd81d3542a419d6")]
        [TestCase("The quick brown fox jumps over the lazy dog.", "e4d909c290d0fb1ca068ffaddf22cbd0")]
        [TestCase("", "d41d8cd98f00b204e9800998ecf8427e")]
        [TestCase("a", "0cc175b9c0f1b6a831c399e269772661")]
        [TestCase("abc", "900150983cd24fb0d6963f7d28e17f72")]
        [TestCase("message digest", "f96b697d7cb7938d525a2f31aaf161d0")]
        [TestCase("abcdefghijklmnopqrstuvwxyz", "c3fcd3d76192e4007dfb496cca67e13b")]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", "d174ab98d277d9f5a5611c2c9f419d9f")]
        [TestCase("12345678901234567890123456789012345678901234567890123456789012345678901234567890", "57edf4a22be3c955ac49da2e2107b67a")]
        public void Test1(string input, string output)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes;

            using (var md5 = StxWright.MD5.Create())
            {
                hashBytes = md5.ComputeHash(inputBytes);
            }

            Assert.That(Convert.ToHexString(hashBytes), Is.EqualTo(output).IgnoreCase);
        }
    }
}