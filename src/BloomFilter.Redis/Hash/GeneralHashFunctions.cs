/*
 * reference http://www.partow.net/programming/hashfunctions/index.html
 */

using System.Diagnostics.CodeAnalysis;

namespace BloomFilter.Redis
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class GeneralHashFunctions
    {
        public static ulong RSHash(string value)
        {
            var a = 378551U;
            var b = 63689U;
            var hash = 0UL;

            foreach (var ch in value)
            {
                hash = hash * a + ch;
                a = a * b;
            }

            return hash;
        }

        public static ulong JSHash(string value)
        {
            var hash = 1315423911UL;

            foreach (var ch in value)
            {
                hash ^= (hash << 5) + ch + (hash >> 2);
            }

            return hash;
        }

        public static ulong PJWHash(string value)
        {
            var bitsInUnsignedInt = 4 * 8;
            var threeQuarters = (bitsInUnsignedInt * 3) / 4;
            var oneEighth = bitsInUnsignedInt / 8;
            var highBits = 0xFFFFFFFFUL << (bitsInUnsignedInt - oneEighth);
            var hash = 0UL;
            var test = 0UL;

            foreach (var ch in value)
            {
                hash = (hash << oneEighth) + ch;

                if ((test = hash & highBits) != 0)
                {
                    hash = (hash ^ (test >> threeQuarters)) & (~highBits);
                }
            }

            return hash;
        }

        public static ulong ELFHash(string value)
        {
            var hash = 0UL;
            var x = 0UL;

            foreach (var ch in value)
            {
                hash = (hash << 4) + ch;

                if ((x = hash & 0xF0000000L) != 0)
                {
                    hash ^= (x >> 24);
                }

                hash &= ~x;
            }

            return hash;
        }

        public static ulong BKDRHash(string value)
        {
            var seed = 131UL; // 31 131 1313 13131 131313 etc..
            var hash = 0UL;

            foreach (var ch in value)
            {
                hash = (hash * seed) + ch;
            }

            return hash;
        }

        public static ulong SDBMHash(string value)
        {
            var hash = 0UL;

            foreach (var ch in value)
            {
                hash = ch + (hash << 6) + (hash << 16) - hash;
            }

            return hash;
        }

        public static ulong DJBHash(string value)
        {
            var hash = 5381UL;

            foreach (var ch in value)
            {
                hash = ((hash << 5) + hash) + ch;
            }

            return hash;
        }

        public static ulong DEKHash(string value)
        {
            var hash = (ulong) value.Length;

            foreach (var ch in value)
            {
                hash = ((hash << 5) ^ (hash >> 27)) ^ ch;
            }

            return hash;
        }

        public static ulong BPHash(string value)
        {
            var hash = 0UL;

            foreach (var ch in value)
            {
                hash = hash << 7 ^ ch;
            }

            return hash;
        }

        public static ulong FNVHash(string value)
        {
            var fnvPrime = 0x811C9DC5UL;
            var hash = 0UL;

            foreach (var ch in value)
            {
                hash *= fnvPrime;
                hash ^= ch;
            }

            return hash;
        }

        public static ulong APHash(string value)
        {
            var hash = 0xAAAAAAAAUL;

            for (var i = 0; i < value.Length; i++)
            {
                if ((i & 1) == 0)
                {
                    hash ^= (hash << 7) ^ value[i] * (hash >> 3);
                }
                else
                {
                    hash ^= ~((hash << 11) + value[i] ^ (hash >> 5));
                }
            }

            return hash;
        }
    }
}
