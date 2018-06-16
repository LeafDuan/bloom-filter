using System;
using System.Text;

using Murmur;

namespace BloomFilter
{
    public static class MurmurHash3
    {
        public static ulong Hash(string value, uint seed = 0)
        {
            var bytes = MurmurHash.Create128(seed).ComputeHash(Encoding.UTF8.GetBytes(value));

            return BitConverter.ToUInt64(bytes, 0);
        }
    }
}
