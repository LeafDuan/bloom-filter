using System;
using System.Collections;
using System.Collections.Generic;

namespace BloomFilter
{
    public class MemoryBloomFilter
    {
        private readonly IEnumerable<Func<string, ulong>> _hashFuncs;
        private readonly BitArray _bits;

        private static readonly Func<string, ulong>[] Mmh3Funcs =
        {
            value => MurmurHash3.Hash(value, 41),
            value => MurmurHash3.Hash(value, 42),
            value => MurmurHash3.Hash(value, 43),
            value => MurmurHash3.Hash(value, 44),
            value => MurmurHash3.Hash(value, 45),
            value => MurmurHash3.Hash(value, 46),
            value => MurmurHash3.Hash(value, 47),
            value => MurmurHash3.Hash(value, 48)
        };

        public MemoryBloomFilter(int bitSize = 5000000) : this(Mmh3Funcs, bitSize)
        {
        }

        public MemoryBloomFilter(IEnumerable<Func<string, ulong>> hashFuncs, int bitSize)
        {
            this._hashFuncs = hashFuncs ?? throw new ArgumentNullException(nameof(hashFuncs));

            if (bitSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bitSize), "must be greater than zero.");
            }

            this._bits = new BitArray(bitSize);
        }

        public bool TryAdd(string value)
        {
            var hasValue = true;

            foreach (var hashFunc in _hashFuncs)
            {
                var index = (int) (hashFunc(value) % (uint) _bits.Length);
                var exists = _bits[index];

                if (!exists)
                {
                    _bits[index] = true;
                }

                hasValue = hasValue && exists;
            }

            return !hasValue;
        }
    }
}
