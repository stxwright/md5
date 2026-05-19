using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Numerics;

namespace StxWright
{
    public sealed class MD5 : IDisposable
    {
        public static MD5 Create() => new MD5();

        [SkipLocalsInit]
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
                Array.Clear(buffer, originalLength + 1, paddingLength - 1);

                // Set length in bits (little-endian)
                BinaryPrimitives.WriteInt64LittleEndian(buffer.AsSpan(totalLength - 8), (long)originalLength * 8);

                uint a0 = 0x67452301;
                uint b0 = 0xefcdab89;
                uint c0 = 0x98badcfe;
                uint d0 = 0x10325476;

                for (int i = 0; i < totalLength; i += 64)
                {
                    ReadOnlySpan<byte> chunk = buffer.AsSpan(i, 64);

                    uint A = a0, B = b0, C = c0, D = d0;
                    uint v;

                    // Rounds logic
                    // Round 1
                    var m0 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(0 , 4));
                    v = A + (D ^ (B & (C ^ D))) + m0 + 0xd76aa478; A = B + BitOperations.RotateLeft(v, 7);
                    var m1 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(4 , 4));
                    v = D + (C ^ (A & (B ^ C))) + m1 + 0xe8c7b756; D = A + BitOperations.RotateLeft(v, 12);
                    var m2 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(8 , 4));
                    v = C + (B ^ (D & (A ^ B))) + m2 + 0x242070db; C = D + BitOperations.RotateLeft(v, 17);
                    var m3 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(12, 4));
                    v = B + (A ^ (C & (D ^ A))) + m3 + 0xc1bdceee; B = C + BitOperations.RotateLeft(v, 22);
                    var m4 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(16, 4));
                    v = A + (D ^ (B & (C ^ D))) + m4 + 0xf57c0faf; A = B + BitOperations.RotateLeft(v, 7);
                    var m5 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(20, 4));
                    v = D + (C ^ (A & (B ^ C))) + m5 + 0x4787c62a; D = A + BitOperations.RotateLeft(v, 12);
                    var m6 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(24, 4));
                    v = C + (B ^ (D & (A ^ B))) + m6 + 0xa8304613; C = D + BitOperations.RotateLeft(v, 17);
                    var m7 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(28, 4));
                    v = B + (A ^ (C & (D ^ A))) + m7 + 0xfd469501; B = C + BitOperations.RotateLeft(v, 22);
                    var m8 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(32, 4));
                    v = A + (D ^ (B & (C ^ D))) + m8 + 0x698098d8; A = B + BitOperations.RotateLeft(v, 7);
                    var m9 = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(36, 4));
                    v = D + (C ^ (A & (B ^ C))) + m9 + 0x8b44f7af; D = A + BitOperations.RotateLeft(v, 12);
                    var mA = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(40, 4));
                    v = C + (B ^ (D & (A ^ B))) + mA + 0xffff5bb1; C = D + BitOperations.RotateLeft(v, 17);
                    var mB = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(44, 4));
                    v = B + (A ^ (C & (D ^ A))) + mB + 0x895cd7be; B = C + BitOperations.RotateLeft(v, 22);
                    var mC = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(48, 4));
                    v = A + (D ^ (B & (C ^ D))) + mC + 0x6b901122; A = B + BitOperations.RotateLeft(v, 7);
                    var mD = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(52, 4));
                    v = D + (C ^ (A & (B ^ C))) + mD + 0xfd987193; D = A + BitOperations.RotateLeft(v, 12);
                    var mE = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(56, 4));
                    v = C + (B ^ (D & (A ^ B))) + mE + 0xa679438e; C = D + BitOperations.RotateLeft(v, 17);
                    var mF = BinaryPrimitives.ReadUInt32LittleEndian(chunk.Slice(60, 4));
                    v = B + (A ^ (C & (D ^ A))) + mF + 0x49b40821; B = C + BitOperations.RotateLeft(v, 22);

                    // Round 2
                    v = A + (C ^ (D & (B ^ C))) + m1 + 0xf61e2562; A = B + BitOperations.RotateLeft(v, 5);
                    v = D + (B ^ (C & (A ^ B))) + m6 + 0xc040b340; D = A + BitOperations.RotateLeft(v, 9);
                    v = C + (A ^ (B & (D ^ A))) + mB + 0x265e5a51; C = D + BitOperations.RotateLeft(v, 14);
                    v = B + (D ^ (A & (C ^ D))) + m0 + 0xe9b6c7aa; B = C + BitOperations.RotateLeft(v, 20);
                    v = A + (C ^ (D & (B ^ C))) + m5 + 0xd62f105d; A = B + BitOperations.RotateLeft(v, 5);
                    v = D + (B ^ (C & (A ^ B))) + mA + 0x02441453; D = A + BitOperations.RotateLeft(v, 9);
                    v = C + (A ^ (B & (D ^ A))) + mF + 0xd8a1e681; C = D + BitOperations.RotateLeft(v, 14);
                    v = B + (D ^ (A & (C ^ D))) + m4 + 0xe7d3fbc8; B = C + BitOperations.RotateLeft(v, 20);
                    v = A + (C ^ (D & (B ^ C))) + m9 + 0x21e1cde6; A = B + BitOperations.RotateLeft(v, 5);
                    v = D + (B ^ (C & (A ^ B))) + mE + 0xc33707d6; D = A + BitOperations.RotateLeft(v, 9);
                    v = C + (A ^ (B & (D ^ A))) + m3 + 0xf4d50d87; C = D + BitOperations.RotateLeft(v, 14);
                    v = B + (D ^ (A & (C ^ D))) + m8 + 0x455a14ed; B = C + BitOperations.RotateLeft(v, 20);
                    v = A + (C ^ (D & (B ^ C))) + mD + 0xa9e3e905; A = B + BitOperations.RotateLeft(v, 5);
                    v = D + (B ^ (C & (A ^ B))) + m2 + 0xfcefa3f8; D = A + BitOperations.RotateLeft(v, 9);
                    v = C + (A ^ (B & (D ^ A))) + m7 + 0x676f02d9; C = D + BitOperations.RotateLeft(v, 14);
                    v = B + (D ^ (A & (C ^ D))) + mC + 0x8d2a4c8a; B = C + BitOperations.RotateLeft(v, 20);

                    // Round 3
                    v = A + (B ^ C ^ D) + m5 + 0xfffa3942; A = B + BitOperations.RotateLeft(v, 4);
                    v = D + (A ^ B ^ C) + m8 + 0x8771f681; D = A + BitOperations.RotateLeft(v, 11);
                    v = C + (D ^ A ^ B) + mB + 0x6d9d6122; C = D + BitOperations.RotateLeft(v, 16);
                    v = B + (C ^ D ^ A) + mE + 0xfde5380c; B = C + BitOperations.RotateLeft(v, 23);
                    v = A + (B ^ C ^ D) + m1 + 0xa4beea44; A = B + BitOperations.RotateLeft(v, 4);
                    v = D + (A ^ B ^ C) + m4 + 0x4bdecfa9; D = A + BitOperations.RotateLeft(v, 11);
                    v = C + (D ^ A ^ B) + m7 + 0xf6bb4b60; C = D + BitOperations.RotateLeft(v, 16);
                    v = B + (C ^ D ^ A) + mA + 0xbebfbc70; B = C + BitOperations.RotateLeft(v, 23);
                    v = A + (B ^ C ^ D) + mD + 0x289b7ec6; A = B + BitOperations.RotateLeft(v, 4);
                    v = D + (A ^ B ^ C) + m0 + 0xeaa127fa; D = A + BitOperations.RotateLeft(v, 11);
                    v = C + (D ^ A ^ B) + m3 + 0xd4ef3085; C = D + BitOperations.RotateLeft(v, 16);
                    v = B + (C ^ D ^ A) + m6 + 0x04881d05; B = C + BitOperations.RotateLeft(v, 23);
                    v = A + (B ^ C ^ D) + m9 + 0xd9d4d039; A = B + BitOperations.RotateLeft(v, 4);
                    v = D + (A ^ B ^ C) + mC + 0xe6db99e5; D = A + BitOperations.RotateLeft(v, 11);
                    v = C + (D ^ A ^ B) + mF + 0x1fa27cf8; C = D + BitOperations.RotateLeft(v, 16);
                    v = B + (C ^ D ^ A) + m2 + 0xc4ac5665; B = C + BitOperations.RotateLeft(v, 23);

                    // Round 4
                    v = A + (C ^ (B | ~D)) + m0 + 0xf4292244; A = B + BitOperations.RotateLeft(v, 6);
                    v = D + (B ^ (A | ~C)) + m7 + 0x432aff97; D = A + BitOperations.RotateLeft(v, 10);
                    v = C + (A ^ (D | ~B)) + mE + 0xab9423a7; C = D + BitOperations.RotateLeft(v, 15);
                    v = B + (D ^ (C | ~A)) + m5 + 0xfc93a039; B = C + BitOperations.RotateLeft(v, 21);
                    v = A + (C ^ (B | ~D)) + mC + 0x655b59c3; A = B + BitOperations.RotateLeft(v, 6);
                    v = D + (B ^ (A | ~C)) + m3 + 0x8f0ccc92; D = A + BitOperations.RotateLeft(v, 10);
                    v = C + (A ^ (D | ~B)) + mA + 0xffeff47d; C = D + BitOperations.RotateLeft(v, 15);
                    v = B + (D ^ (C | ~A)) + m1 + 0x85845dd1; B = C + BitOperations.RotateLeft(v, 21);
                    v = A + (C ^ (B | ~D)) + m8 + 0x6fa87e4f; A = B + BitOperations.RotateLeft(v, 6);
                    v = D + (B ^ (A | ~C)) + mF + 0xfe2ce6e0; D = A + BitOperations.RotateLeft(v, 10);
                    v = C + (A ^ (D | ~B)) + m6 + 0xa3014314; C = D + BitOperations.RotateLeft(v, 15);
                    v = B + (D ^ (C | ~A)) + mD + 0x4e0811a1; B = C + BitOperations.RotateLeft(v, 21);
                    v = A + (C ^ (B | ~D)) + m4 + 0xf7537e82; A = B + BitOperations.RotateLeft(v, 6);
                    v = D + (B ^ (A | ~C)) + mB + 0xbd3af235; D = A + BitOperations.RotateLeft(v, 10);
                    v = C + (A ^ (D | ~B)) + m2 + 0x2ad7d2bb; C = D + BitOperations.RotateLeft(v, 15);
                    v = B + (D ^ (C | ~A)) + m9 + 0xeb86d391; B = C + BitOperations.RotateLeft(v, 21);

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