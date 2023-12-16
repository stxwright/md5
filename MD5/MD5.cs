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
                v = A + ((B & C) | (~B & D)) + M[0x0] + 0xd76aa478; A = B + (v << 7 | v >> 25);
                v = D + ((A & B) | (~A & C)) + M[0x1] + 0xe8c7b756; D = A + (v << 12 | v >> 20);
                v = C + ((D & A) | (~D & B)) + M[0x2] + 0x242070db; C = D + (v << 17 | v >> 15);
                v = B + ((C & D) | (~C & A)) + M[0x3] + 0xc1bdceee; B = C + (v << 22 | v >> 10);
                v = A + ((B & C) | (~B & D)) + M[0x4] + 0xf57c0faf; A = B + (v << 7 | v >> 25);
                v = D + ((A & B) | (~A & C)) + M[0x5] + 0x4787c62a; D = A + (v << 12 | v >> 20);
                v = C + ((D & A) | (~D & B)) + M[0x6] + 0xa8304613; C = D + (v << 17 | v >> 15);
                v = B + ((C & D) | (~C & A)) + M[0x7] + 0xfd469501; B = C + (v << 22 | v >> 10);
                v = A + ((B & C) | (~B & D)) + M[0x8] + 0x698098d8; A = B + (v << 7 | v >> 25);
                v = D + ((A & B) | (~A & C)) + M[0x9] + 0x8b44f7af; D = A + (v << 12 | v >> 20);
                v = C + ((D & A) | (~D & B)) + M[0xA] + 0xffff5bb1; C = D + (v << 17 | v >> 15);
                v = B + ((C & D) | (~C & A)) + M[0xB] + 0x895cd7be; B = C + (v << 22 | v >> 10);
                v = A + ((B & C) | (~B & D)) + M[0xC] + 0x6b901122; A = B + (v << 7 | v >> 25);
                v = D + ((A & B) | (~A & C)) + M[0xD] + 0xfd987193; D = A + (v << 12 | v >> 20);
                v = C + ((D & A) | (~D & B)) + M[0xE] + 0xa679438e; C = D + (v << 17 | v >> 15);
                v = B + ((C & D) | (~C & A)) + M[0xF] + 0x49b40821; B = C + (v << 22 | v >> 10);

                v = A + ((D & B) | (~D & C)) + M[0x1] + 0xf61e2562; A = B + (v << 5 | v >> 27);
                v = D + ((C & A) | (~C & B)) + M[0x6] + 0xc040b340; D = A + (v << 9 | v >> 23);
                v = C + ((B & D) | (~B & A)) + M[0xB] + 0x265e5a51; C = D + (v << 14 | v >> 18);
                v = B + ((A & C) | (~A & D)) + M[0x0] + 0xe9b6c7aa; B = C + (v << 20 | v >> 12);
                v = A + ((D & B) | (~D & C)) + M[0x5] + 0xd62f105d; A = B + (v << 5 | v >> 27);
                v = D + ((C & A) | (~C & B)) + M[0xA] + 0x02441453; D = A + (v << 9 | v >> 23);
                v = C + ((B & D) | (~B & A)) + M[0xF] + 0xd8a1e681; C = D + (v << 14 | v >> 18);
                v = B + ((A & C) | (~A & D)) + M[0x4] + 0xe7d3fbc8; B = C + (v << 20 | v >> 12);
                v = A + ((D & B) | (~D & C)) + M[0x9] + 0x21e1cde6; A = B + (v << 5 | v >> 27);
                v = D + ((C & A) | (~C & B)) + M[0xE] + 0xc33707d6; D = A + (v << 9 | v >> 23);
                v = C + ((B & D) | (~B & A)) + M[0x3] + 0xf4d50d87; C = D + (v << 14 | v >> 18);
                v = B + ((A & C) | (~A & D)) + M[0x8] + 0x455a14ed; B = C + (v << 20 | v >> 12);
                v = A + ((D & B) | (~D & C)) + M[0xD] + 0xa9e3e905; A = B + (v << 5 | v >> 27);
                v = D + ((C & A) | (~C & B)) + M[0x2] + 0xfcefa3f8; D = A + (v << 9 | v >> 23);
                v = C + ((B & D) | (~B & A)) + M[0x7] + 0x676f02d9; C = D + (v << 14 | v >> 18);
                v = B + ((A & C) | (~A & D)) + M[0xC] + 0x8d2a4c8a; B = C + (v << 20 | v >> 12);

                v = A + (B ^ C ^ D) + M[0x5] + 0xfffa3942; A = B + (v << 4 | v >> 28);
                v = D + (A ^ B ^ C) + M[0x8] + 0x8771f681; D = A + (v << 11 | v >> 21);
                v = C + (D ^ A ^ B) + M[0xB] + 0x6d9d6122; C = D + (v << 16 | v >> 16);
                v = B + (C ^ D ^ A) + M[0xE] + 0xfde5380c; B = C + (v << 23 | v >> 9);
                v = A + (B ^ C ^ D) + M[0x1] + 0xa4beea44; A = B + (v << 4 | v >> 28);
                v = D + (A ^ B ^ C) + M[0x4] + 0x4bdecfa9; D = A + (v << 11 | v >> 21);
                v = C + (D ^ A ^ B) + M[0x7] + 0xf6bb4b60; C = D + (v << 16 | v >> 16);
                v = B + (C ^ D ^ A) + M[0xA] + 0xbebfbc70; B = C + (v << 23 | v >> 9);
                v = A + (B ^ C ^ D) + M[0xD] + 0x289b7ec6; A = B + (v << 4 | v >> 28);
                v = D + (A ^ B ^ C) + M[0x0] + 0xeaa127fa; D = A + (v << 11 | v >> 21);
                v = C + (D ^ A ^ B) + M[0x3] + 0xd4ef3085; C = D + (v << 16 | v >> 16);
                v = B + (C ^ D ^ A) + M[0x6] + 0x04881d05; B = C + (v << 23 | v >> 9);
                v = A + (B ^ C ^ D) + M[0x9] + 0xd9d4d039; A = B + (v << 4 | v >> 28);
                v = D + (A ^ B ^ C) + M[0xC] + 0xe6db99e5; D = A + (v << 11 | v >> 21);
                v = C + (D ^ A ^ B) + M[0xF] + 0x1fa27cf8; C = D + (v << 16 | v >> 16);
                v = B + (C ^ D ^ A) + M[0x2] + 0xc4ac5665; B = C + (v << 23 | v >> 9);

                v = A + (C ^ (B | ~D)) + M[0x0] + 0xf4292244; A = B + (v << 6 | v >> 26);
                v = D + (B ^ (A | ~C)) + M[0x7] + 0x432aff97; D = A + (v << 10 | v >> 22);
                v = C + (A ^ (D | ~B)) + M[0xE] + 0xab9423a7; C = D + (v << 15 | v >> 17);
                v = B + (D ^ (C | ~A)) + M[0x5] + 0xfc93a039; B = C + (v << 21 | v >> 11);
                v = A + (C ^ (B | ~D)) + M[0xC] + 0x655b59c3; A = B + (v << 6 | v >> 26);
                v = D + (B ^ (A | ~C)) + M[0x3] + 0x8f0ccc92; D = A + (v << 10 | v >> 22);
                v = C + (A ^ (D | ~B)) + M[0xA] + 0xffeff47d; C = D + (v << 15 | v >> 17);
                v = B + (D ^ (C | ~A)) + M[0x1] + 0x85845dd1; B = C + (v << 21 | v >> 11);
                v = A + (C ^ (B | ~D)) + M[0x8] + 0x6fa87e4f; A = B + (v << 6 | v >> 26);
                v = D + (B ^ (A | ~C)) + M[0xF] + 0xfe2ce6e0; D = A + (v << 10 | v >> 22);
                v = C + (A ^ (D | ~B)) + M[0x6] + 0xa3014314; C = D + (v << 15 | v >> 17);
                v = B + (D ^ (C | ~A)) + M[0xD] + 0x4e0811a1; B = C + (v << 21 | v >> 11);
                v = A + (C ^ (B | ~D)) + M[0x4] + 0xf7537e82; A = B + (v << 6 | v >> 26);
                v = D + (B ^ (A | ~C)) + M[0xB] + 0xbd3af235; D = A + (v << 10 | v >> 22);
                v = C + (A ^ (D | ~B)) + M[0x2] + 0x2ad7d2bb; C = D + (v << 15 | v >> 17);
                v = B + (D ^ (C | ~A)) + M[0x9] + 0xeb86d391; B = C + (v << 21 | v >> 11);

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