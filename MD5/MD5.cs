using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

            uint[] M = new uint[16];
            for (int i = 0; i < processedInput.Length / 64; ++i)
            {
                // copy the input to M
                for (int j = 0; j < 16; ++j)
                    M[j] = BitConverter.ToUInt32(processedInput, (i * 64) + (j * 4));

                // initialize round variables
                uint A = a0, B = b0, C = c0, D = d0;

                // temporary variables
                uint v;

                // primary loop
                v = A + ((B & C) | (~B & D)) + M[0x0] + 0xd76aa478; A = D; D = C; C = B; B += v << 7 | v >> 25;
                v = A + ((B & C) | (~B & D)) + M[0x1] + 0xe8c7b756; A = D; D = C; C = B; B += v << 12 | v >> 20;
                v = A + ((B & C) | (~B & D)) + M[0x2] + 0x242070db; A = D; D = C; C = B; B += v << 17 | v >> 15;
                v = A + ((B & C) | (~B & D)) + M[0x3] + 0xc1bdceee; A = D; D = C; C = B; B += v << 22 | v >> 10;
                v = A + ((B & C) | (~B & D)) + M[0x4] + 0xf57c0faf; A = D; D = C; C = B; B += v << 7 | v >> 25;
                v = A + ((B & C) | (~B & D)) + M[0x5] + 0x4787c62a; A = D; D = C; C = B; B += v << 12 | v >> 20;
                v = A + ((B & C) | (~B & D)) + M[0x6] + 0xa8304613; A = D; D = C; C = B; B += v << 17 | v >> 15;
                v = A + ((B & C) | (~B & D)) + M[0x7] + 0xfd469501; A = D; D = C; C = B; B += v << 22 | v >> 10;
                v = A + ((B & C) | (~B & D)) + M[0x8] + 0x698098d8; A = D; D = C; C = B; B += v << 7 | v >> 25;
                v = A + ((B & C) | (~B & D)) + M[0x9] + 0x8b44f7af; A = D; D = C; C = B; B += v << 12 | v >> 20;
                v = A + ((B & C) | (~B & D)) + M[0xA] + 0xffff5bb1; A = D; D = C; C = B; B += v << 17 | v >> 15;
                v = A + ((B & C) | (~B & D)) + M[0xB] + 0x895cd7be; A = D; D = C; C = B; B += v << 22 | v >> 10;
                v = A + ((B & C) | (~B & D)) + M[0xC] + 0x6b901122; A = D; D = C; C = B; B += v << 7 | v >> 25;
                v = A + ((B & C) | (~B & D)) + M[0xD] + 0xfd987193; A = D; D = C; C = B; B += v << 12 | v >> 20;
                v = A + ((B & C) | (~B & D)) + M[0xE] + 0xa679438e; A = D; D = C; C = B; B += v << 17 | v >> 15;
                v = A + ((B & C) | (~B & D)) + M[0xF] + 0x49b40821; A = D; D = C; C = B; B += v << 22 | v >> 10;

                v = A + ((D & B) | (~D & C)) + M[0x1] + 0xf61e2562; A = D; D = C; C = B; B += v << 5 | v >> 27;
                v = A + ((D & B) | (~D & C)) + M[0x6] + 0xc040b340; A = D; D = C; C = B; B += v << 9 | v >> 23;
                v = A + ((D & B) | (~D & C)) + M[0xB] + 0x265e5a51; A = D; D = C; C = B; B += v << 14 | v >> 18;
                v = A + ((D & B) | (~D & C)) + M[0x0] + 0xe9b6c7aa; A = D; D = C; C = B; B += v << 20 | v >> 12;
                v = A + ((D & B) | (~D & C)) + M[0x5] + 0xd62f105d; A = D; D = C; C = B; B += v << 5 | v >> 27;
                v = A + ((D & B) | (~D & C)) + M[0xA] + 0x02441453; A = D; D = C; C = B; B += v << 9 | v >> 23;
                v = A + ((D & B) | (~D & C)) + M[0xF] + 0xd8a1e681; A = D; D = C; C = B; B += v << 14 | v >> 18;
                v = A + ((D & B) | (~D & C)) + M[0x4] + 0xe7d3fbc8; A = D; D = C; C = B; B += v << 20 | v >> 12;
                v = A + ((D & B) | (~D & C)) + M[0x9] + 0x21e1cde6; A = D; D = C; C = B; B += v << 5 | v >> 27;
                v = A + ((D & B) | (~D & C)) + M[0xE] + 0xc33707d6; A = D; D = C; C = B; B += v << 9 | v >> 23;
                v = A + ((D & B) | (~D & C)) + M[0x3] + 0xf4d50d87; A = D; D = C; C = B; B += v << 14 | v >> 18;
                v = A + ((D & B) | (~D & C)) + M[0x8] + 0x455a14ed; A = D; D = C; C = B; B += v << 20 | v >> 12;
                v = A + ((D & B) | (~D & C)) + M[0xD] + 0xa9e3e905; A = D; D = C; C = B; B += v << 5 | v >> 27;
                v = A + ((D & B) | (~D & C)) + M[0x2] + 0xfcefa3f8; A = D; D = C; C = B; B += v << 9 | v >> 23;
                v = A + ((D & B) | (~D & C)) + M[0x7] + 0x676f02d9; A = D; D = C; C = B; B += v << 14 | v >> 18;
                v = A + ((D & B) | (~D & C)) + M[0xC] + 0x8d2a4c8a; A = D; D = C; C = B; B += v << 20 | v >> 12;

                v = A + (B ^ C ^ D) + M[0x5] + 0xfffa3942; A = D; D = C; C = B; B += v << 4 | v >> 28;
                v = A + (B ^ C ^ D) + M[0x8] + 0x8771f681; A = D; D = C; C = B; B += v << 11 | v >> 21;
                v = A + (B ^ C ^ D) + M[0xB] + 0x6d9d6122; A = D; D = C; C = B; B += v << 16 | v >> 16;
                v = A + (B ^ C ^ D) + M[0xE] + 0xfde5380c; A = D; D = C; C = B; B += v << 23 | v >> 9;
                v = A + (B ^ C ^ D) + M[0x1] + 0xa4beea44; A = D; D = C; C = B; B += v << 4 | v >> 28;
                v = A + (B ^ C ^ D) + M[0x4] + 0x4bdecfa9; A = D; D = C; C = B; B += v << 11 | v >> 21;
                v = A + (B ^ C ^ D) + M[0x7] + 0xf6bb4b60; A = D; D = C; C = B; B += v << 16 | v >> 16;
                v = A + (B ^ C ^ D) + M[0xA] + 0xbebfbc70; A = D; D = C; C = B; B += v << 23 | v >> 9;
                v = A + (B ^ C ^ D) + M[0xD] + 0x289b7ec6; A = D; D = C; C = B; B += v << 4 | v >> 28;
                v = A + (B ^ C ^ D) + M[0x0] + 0xeaa127fa; A = D; D = C; C = B; B += v << 11 | v >> 21;
                v = A + (B ^ C ^ D) + M[0x3] + 0xd4ef3085; A = D; D = C; C = B; B += v << 16 | v >> 16;
                v = A + (B ^ C ^ D) + M[0x6] + 0x04881d05; A = D; D = C; C = B; B += v << 23 | v >> 9;
                v = A + (B ^ C ^ D) + M[0x9] + 0xd9d4d039; A = D; D = C; C = B; B += v << 4 | v >> 28;
                v = A + (B ^ C ^ D) + M[0xC] + 0xe6db99e5; A = D; D = C; C = B; B += v << 11 | v >> 21;
                v = A + (B ^ C ^ D) + M[0xF] + 0x1fa27cf8; A = D; D = C; C = B; B += v << 16 | v >> 16;
                v = A + (B ^ C ^ D) + M[0x2] + 0xc4ac5665; A = D; D = C; C = B; B += v << 23 | v >> 9;

                v = A + (C ^ (B | ~D)) + M[0x0] + 0xf4292244; A = D; D = C; C = B; B += v << 6 | v >> 26;
                v = A + (C ^ (B | ~D)) + M[0x7] + 0x432aff97; A = D; D = C; C = B; B += v << 10 | v >> 22;
                v = A + (C ^ (B | ~D)) + M[0xE] + 0xab9423a7; A = D; D = C; C = B; B += v << 15 | v >> 17;
                v = A + (C ^ (B | ~D)) + M[0x5] + 0xfc93a039; A = D; D = C; C = B; B += v << 21 | v >> 11;
                v = A + (C ^ (B | ~D)) + M[0xC] + 0x655b59c3; A = D; D = C; C = B; B += v << 6 | v >> 26;
                v = A + (C ^ (B | ~D)) + M[0x3] + 0x8f0ccc92; A = D; D = C; C = B; B += v << 10 | v >> 22;
                v = A + (C ^ (B | ~D)) + M[0xA] + 0xffeff47d; A = D; D = C; C = B; B += v << 15 | v >> 17;
                v = A + (C ^ (B | ~D)) + M[0x1] + 0x85845dd1; A = D; D = C; C = B; B += v << 21 | v >> 11;
                v = A + (C ^ (B | ~D)) + M[0x8] + 0x6fa87e4f; A = D; D = C; C = B; B += v << 6 | v >> 26;
                v = A + (C ^ (B | ~D)) + M[0xF] + 0xfe2ce6e0; A = D; D = C; C = B; B += v << 10 | v >> 22;
                v = A + (C ^ (B | ~D)) + M[0x6] + 0xa3014314; A = D; D = C; C = B; B += v << 15 | v >> 17;
                v = A + (C ^ (B | ~D)) + M[0xD] + 0x4e0811a1; A = D; D = C; C = B; B += v << 21 | v >> 11;
                v = A + (C ^ (B | ~D)) + M[0x4] + 0xf7537e82; A = D; D = C; C = B; B += v << 6 | v >> 26;
                v = A + (C ^ (B | ~D)) + M[0xB] + 0xbd3af235; A = D; D = C; C = B; B += v << 10 | v >> 22;
                v = A + (C ^ (B | ~D)) + M[0x2] + 0x2ad7d2bb; A = D; D = C; C = B; B += v << 15 | v >> 17;
                v = A + (C ^ (B | ~D)) + M[0x9] + 0xeb86d391; A = D; D = C; C = B; B += v << 21 | v >> 11;

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