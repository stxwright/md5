using System;
using System.Collections.Generic;

namespace StxWright
{
    public sealed class MD5 : IDisposable
    {
        public static MD5 Create()
        {
            return new MD5();
        }

        public byte[] ComputeHash(byte[] inputBytes)
        {
            uint a0 = 0x67452301;   // A
            uint b0 = 0xefcdab89;   // B
            uint c0 = 0x98badcfe;   // C
            uint d0 = 0x10325476;   // D

            var processedInputBuilder = new List<byte>(inputBytes) { 0x80 };
            while (processedInputBuilder.Count % 64 != 56) processedInputBuilder.Add(0x0);
            processedInputBuilder.AddRange(BitConverter.GetBytes((long)inputBytes.Length * 8)); // bit converter returns little-endian
            var processedInput = processedInputBuilder.ToArray();

            byte[] length = BitConverter.GetBytes(inputBytes.Length * 8); // bit converter returns little-endian
            Array.Copy(length, 0, processedInput, processedInput.Length - 8, 4); // add length in bits

            Span<uint> M = stackalloc uint[16];
            for (int i = 0; i < processedInput.Length / 64; ++i)
            {
                // copy the input to M
                for (int j = 0; j < 16; ++j)
                    M[j] = BitConverter.ToUInt32(processedInput, (i * 64) + (j * 4));

                // initialize round variables
                uint A = a0, B = b0, C = c0, D = d0;

                // primary loop
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x0] + 0xd76aa478, 7);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x1] + 0xe8c7b756, 12);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x2] + 0x242070db, 17);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x3] + 0xc1bdceee, 22);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x4] + 0xf57c0faf, 7 );
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x5] + 0x4787c62a, 12);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x6] + 0xa8304613, 17);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x7] + 0xfd469501, 22);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x8] + 0x698098d8, 7 );
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0x9] + 0x8b44f7af, 12);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0xA] + 0xffff5bb1, 17);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0xB] + 0x895cd7be, 22);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0xC] + 0x6b901122, 7 );
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0xD] + 0xfd987193, 12);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0xE] + 0xa679438e, 17);
                EveryTime(ref A, ref B, ref C, ref D, ((B & C) | (~B & D)) + M[0xF] + 0x49b40821, 22);

                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x1] + 0xf61e2562, 5);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x6] + 0xc040b340, 9);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0xB] + 0x265e5a51, 14);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x0] + 0xe9b6c7aa, 20);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x5] + 0xd62f105d, 5);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0xA] + 0x02441453, 9);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0xF] + 0xd8a1e681, 14);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x4] + 0xe7d3fbc8, 20);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x9] + 0x21e1cde6, 5);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0xE] + 0xc33707d6, 9);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x3] + 0xf4d50d87, 14);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x8] + 0x455a14ed, 20);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0xD] + 0xa9e3e905, 5);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x2] + 0xfcefa3f8, 9);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0x7] + 0x676f02d9, 14);
                EveryTime(ref A, ref B, ref C, ref D, ((D & B) | (~D & C)) + M[0xC] + 0x8d2a4c8a, 20);

                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x5] + 0xfffa3942, 4);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x8] + 0x8771f681, 11);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0xB] + 0x6d9d6122, 16);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0xE] + 0xfde5380c, 23);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x1] + 0xa4beea44, 4);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x4] + 0x4bdecfa9, 11);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x7] + 0xf6bb4b60, 16);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0xA] + 0xbebfbc70, 23);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0xD] + 0x289b7ec6, 4);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x0] + 0xeaa127fa, 11);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x3] + 0xd4ef3085, 16);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x6] + 0x04881d05, 23);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x9] + 0xd9d4d039, 4);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0xC] + 0xe6db99e5, 11);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0xF] + 0x1fa27cf8, 16);
                EveryTime(ref A, ref B, ref C, ref D, (B ^ C ^ D) + M[0x2] + 0xc4ac5665, 23);

                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x0] + 0xf4292244, 6);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x7] + 0x432aff97, 10);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0xE] + 0xab9423a7, 15);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x5] + 0xfc93a039, 21);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0xC] + 0x655b59c3, 6);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x3] + 0x8f0ccc92, 10);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0xA] + 0xffeff47d, 15);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x1] + 0x85845dd1, 21);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x8] + 0x6fa87e4f, 6);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0xF] + 0xfe2ce6e0, 10);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x6] + 0xa3014314, 15);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0xD] + 0x4e0811a1, 21);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x4] + 0xf7537e82, 6);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0xB] + 0xbd3af235, 10);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x2] + 0x2ad7d2bb, 15);
                EveryTime(ref A, ref B, ref C, ref D, (C ^ (B | ~D)) + M[0x9] + 0xeb86d391, 21);

                static void EveryTime(ref uint A, ref uint B, ref uint C, ref uint D, uint k, int s)
                {
                    uint v = A + k;
                    A = D;
                    D = C;
                    C = B;
                    B += v << s | v >> (32 - s);
                }

                a0 += A;
                b0 += B;
                c0 += C;
                d0 += D;
            }

            byte[] md5 = new byte[16];
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                int n = (int)((i == 0) ? a0 : ((i == 1) ? b0 : ((i == 2) ? c0 : d0)));
                for (int j = 0; j < 4; j++)
                {
                    md5[count++] = (byte)n;
                    n >>>= 8;
                }
            }
            return md5;
        }

        public void Dispose()
        {
        }
    }
}
