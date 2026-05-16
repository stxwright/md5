using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace StxWright
{
    public sealed class MD5 : IDisposable
    {
        public static MD5 Create() => new MD5();

        public byte[] ComputeHash(byte[] inputBytes)
        {
            int originalLength = inputBytes.Length;
            int paddingLength = (56 - (originalLength % 64) + 64) % 64;
            if (paddingLength == 0) paddingLength = 64;
            int totalLength = originalLength + paddingLength + 8;

            byte[] buffer = ArrayPool<byte>.Shared.Rent(totalLength);
            try
            {
                inputBytes.AsSpan().CopyTo(buffer);
                buffer[originalLength] = 0x80;
                
                // Clear the padding area
                Array.Clear(buffer, originalLength + 1, totalLength - originalLength - 1);

                // Set length in bits (little-endian)
                BinaryPrimitives.WriteInt64LittleEndian(buffer.AsSpan(totalLength - 8), (long)originalLength * 8);

                uint a0 = 0x67452301;
                uint b0 = 0xefcdab89;
                uint c0 = 0x98badcfe;
                uint d0 = 0x10325476;

                Span<uint> M = stackalloc uint[16];
                for (int i = 0; i < totalLength; i += 64)
                {
                    ReadOnlySpan<byte> chunk = buffer.AsSpan(i, 64);
                    uint A = a0, B = b0, C = c0, D = d0;
                    uint v;

                    if (BitConverter.IsLittleEndian)
                    {
                        MemoryMarshal.Cast<byte, uint>(chunk).CopyTo(M);
                    }
                    else
                    {
                        for (int j = 0; j < 16; j++)
                            M[j] = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(j * 4, 4));
                    }

                    // Rounds logic
                    // Round 1
                    v = A + (D ^ (B & (C ^ D))) + M[0x0] + 0xd76aa478; A = B + ((v << 7) | (v >> 25));
                    v = D + (C ^ (A & (B ^ C))) + M[0x1] + 0xe8c7b756; D = A + ((v << 12) | (v >> 20));
                    v = C + (B ^ (D & (A ^ B))) + M[0x2] + 0x242070db; C = D + ((v << 17) | (v >> 15));
                    v = B + (A ^ (C & (D ^ A))) + M[0x3] + 0xc1bdceee; B = C + ((v << 22) | (v >> 10));
                    v = A + (D ^ (B & (C ^ D))) + M[0x4] + 0xf57c0faf; A = B + ((v << 7) | (v >> 25));
                    v = D + (C ^ (A & (B ^ C))) + M[0x5] + 0x4787c62a; D = A + ((v << 12) | (v >> 20));
                    v = C + (B ^ (D & (A ^ B))) + M[0x6] + 0xa8304613; C = D + ((v << 17) | (v >> 15));
                    v = B + (A ^ (C & (D ^ A))) + M[0x7] + 0xfd469501; B = C + ((v << 22) | (v >> 10));
                    v = A + (D ^ (B & (C ^ D))) + M[0x8] + 0x698098d8; A = B + ((v << 7) | (v >> 25));
                    v = D + (C ^ (A & (B ^ C))) + M[0x9] + 0x8b44f7af; D = A + ((v << 12) | (v >> 20));
                    v = C + (B ^ (D & (A ^ B))) + M[0xA] + 0xffff5bb1; C = D + ((v << 17) | (v >> 15));
                    v = B + (A ^ (C & (D ^ A))) + M[0xB] + 0x895cd7be; B = C + ((v << 22) | (v >> 10));
                    v = A + (D ^ (B & (C ^ D))) + M[0xC] + 0x6b901122; A = B + ((v << 7) | (v >> 25));
                    v = D + (C ^ (A & (B ^ C))) + M[0xD] + 0xfd987193; D = A + ((v << 12) | (v >> 20));
                    v = C + (B ^ (D & (A ^ B))) + M[0xE] + 0xa679438e; C = D + ((v << 17) | (v >> 15));
                    v = B + (A ^ (C & (D ^ A))) + M[0xF] + 0x49b40821; B = C + ((v << 22) | (v >> 10));

                    // Round 2
                    v = A + (C ^ (D & (B ^ C))) + M[0x1] + 0xf61e2562; A = B + ((v << 5) | (v >> 27));
                    v = D + (B ^ (C & (A ^ B))) + M[0x6] + 0xc040b340; D = A + ((v << 9) | (v >> 23));
                    v = C + (A ^ (B & (D ^ A))) + M[0xB] + 0x265e5a51; C = D + ((v << 14) | (v >> 18));
                    v = B + (D ^ (A & (C ^ D))) + M[0x0] + 0xe9b6c7aa; B = C + ((v << 20) | (v >> 12));
                    v = A + (C ^ (D & (B ^ C))) + M[0x5] + 0xd62f105d; A = B + ((v << 5) | (v >> 27));
                    v = D + (B ^ (C & (A ^ B))) + M[0xA] + 0x02441453; D = A + ((v << 9) | (v >> 23));
                    v = C + (A ^ (B & (D ^ A))) + M[0xF] + 0xd8a1e681; C = D + ((v << 14) | (v >> 18));
                    v = B + (D ^ (A & (C ^ D))) + M[0x4] + 0xe7d3fbc8; B = C + ((v << 20) | (v >> 12));
                    v = A + (C ^ (D & (B ^ C))) + M[0x9] + 0x21e1cde6; A = B + ((v << 5) | (v >> 27));
                    v = D + (B ^ (C & (A ^ B))) + M[0xE] + 0xc33707d6; D = A + ((v << 9) | (v >> 23));
                    v = C + (A ^ (B & (D ^ A))) + M[0x3] + 0xf4d50d87; C = D + ((v << 14) | (v >> 18));
                    v = B + (D ^ (A & (C ^ D))) + M[0x8] + 0x455a14ed; B = C + ((v << 20) | (v >> 12));
                    v = A + (C ^ (D & (B ^ C))) + M[0xD] + 0xa9e3e905; A = B + ((v << 5) | (v >> 27));
                    v = D + (B ^ (C & (A ^ B))) + M[0x2] + 0xfcefa3f8; D = A + ((v << 9) | (v >> 23));
                    v = C + (A ^ (B & (D ^ A))) + M[0x7] + 0x676f02d9; C = D + ((v << 14) | (v >> 18));
                    v = B + (D ^ (A & (C ^ D))) + M[0xC] + 0x8d2a4c8a; B = C + ((v << 20) | (v >> 12));

                    // Round 3
                    v = A + (B ^ C ^ D) + M[0x5] + 0xfffa3942; A = B + ((v << 4) | (v >> 28));
                    v = D + (A ^ B ^ C) + M[0x8] + 0x8771f681; D = A + ((v << 11) | (v >> 21));
                    v = C + (D ^ A ^ B) + M[0xB] + 0x6d9d6122; C = D + ((v << 16) | (v >> 16));
                    v = B + (C ^ D ^ A) + M[0xE] + 0xfde5380c; B = C + ((v << 23) | (v >> 9));
                    v = A + (B ^ C ^ D) + M[0x1] + 0xa4beea44; A = B + ((v << 4) | (v >> 28));
                    v = D + (A ^ B ^ C) + M[0x4] + 0x4bdecfa9; D = A + ((v << 11) | (v >> 21));
                    v = C + (D ^ A ^ B) + M[0x7] + 0xf6bb4b60; C = D + ((v << 16) | (v >> 16));
                    v = B + (C ^ D ^ A) + M[0xA] + 0xbebfbc70; B = C + ((v << 23) | (v >> 9));
                    v = A + (B ^ C ^ D) + M[0xD] + 0x289b7ec6; A = B + ((v << 4) | (v >> 28));
                    v = D + (A ^ B ^ C) + M[0x0] + 0xeaa127fa; D = A + ((v << 11) | (v >> 21));
                    v = C + (D ^ A ^ B) + M[0x3] + 0xd4ef3085; C = D + ((v << 16) | (v >> 16));
                    v = B + (C ^ D ^ A) + M[0x6] + 0x04881d05; B = C + ((v << 23) | (v >> 9));
                    v = A + (B ^ C ^ D) + M[0x9] + 0xd9d4d039; A = B + ((v << 4) | (v >> 28));
                    v = D + (A ^ B ^ C) + M[0xC] + 0xe6db99e5; D = A + ((v << 11) | (v >> 21));
                    v = C + (D ^ A ^ B) + M[0xF] + 0x1fa27cf8; C = D + ((v << 16) | (v >> 16));
                    v = B + (C ^ D ^ A) + M[0x2] + 0xc4ac5665; B = C + ((v << 23) | (v >> 9));

                    // Round 4
                    v = A + (C ^ (B | ~D)) + M[0x0] + 0xf4292244; A = B + ((v << 6) | (v >> 26));
                    v = D + (B ^ (A | ~C)) + M[0x7] + 0x432aff97; D = A + ((v << 10) | (v >> 22));
                    v = C + (A ^ (D | ~B)) + M[0xE] + 0xab9423a7; C = D + ((v << 15) | (v >> 17));
                    v = B + (D ^ (C | ~A)) + M[0x5] + 0xfc93a039; B = C + ((v << 21) | (v >> 11));
                    v = A + (C ^ (B | ~D)) + M[0xC] + 0x655b59c3; A = B + ((v << 6) | (v >> 26));
                    v = D + (B ^ (A | ~C)) + M[0x3] + 0x8f0ccc92; D = A + ((v << 10) | (v >> 22));
                    v = C + (A ^ (D | ~B)) + M[0xA] + 0xffeff47d; C = D + ((v << 15) | (v >> 17));
                    v = B + (D ^ (C | ~A)) + M[0x1] + 0x85845dd1; B = C + ((v << 21) | (v >> 11));
                    v = A + (C ^ (B | ~D)) + M[0x8] + 0x6fa87e4f; A = B + ((v << 6) | (v >> 26));
                    v = D + (B ^ (A | ~C)) + M[0xF] + 0xfe2ce6e0; D = A + ((v << 10) | (v >> 22));
                    v = C + (A ^ (D | ~B)) + M[0x6] + 0xa3014314; C = D + ((v << 15) | (v >> 17));
                    v = B + (D ^ (C | ~A)) + M[0xD] + 0x4e0811a1; B = C + ((v << 21) | (v >> 11));
                    v = A + (C ^ (B | ~D)) + M[0x4] + 0xf7537e82; A = B + ((v << 6) | (v >> 26));
                    v = D + (B ^ (A | ~C)) + M[0xB] + 0xbd3af235; D = A + ((v << 10) | (v >> 22));
                    v = C + (A ^ (D | ~B)) + M[0x2] + 0x2ad7d2bb; C = D + ((v << 15) | (v >> 17));
                    v = B + (D ^ (C | ~A)) + M[0x9] + 0xeb86d391; B = C + ((v << 21) | (v >> 11));

                    a0 += A; b0 += B; c0 += C; d0 += D;
                }
                
                byte[] result = new byte[16];
                BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(0, 4), a0);
                BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(4, 4), b0);
                BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(8, 4), c0);
                BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(12, 4), d0);
                return result;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public void Dispose()
        {
        }
    }
}