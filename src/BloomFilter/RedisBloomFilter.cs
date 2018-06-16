/*
 * reference https://github.com/kongtianyi/BloomFilterRedis
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloomFilter
{
    public class RedisBloomFilter
    {
        private readonly IRedisCache _redisCache;
        private readonly IEnumerable<Func<string, ulong>> _hashFuncs;
        private readonly long _bitSize;

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

        public RedisBloomFilter(IRedisCache redisCache, long bitSize = 5000000) : this(redisCache, Mmh3Funcs, bitSize)
        {
        }

        public RedisBloomFilter(IRedisCache redisCache, IEnumerable<Func<string, ulong>> hashFuncs, long bitSize)
        {
            this._redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            this._hashFuncs = hashFuncs ?? throw new ArgumentNullException(nameof(hashFuncs));

            if (bitSize <= 0 || (1L << 23) < bitSize)
            {
                throw new ArgumentOutOfRangeException(nameof(bitSize), "must be greater than zero and less than 2^32-1.");
            }

            this._bitSize = bitSize;
        }

        public async Task<bool> TryAdd(string key, string value)
        {
            var hasValue = true;

            foreach (var hashFunc in _hashFuncs)
            {
                // 将 hash 函数得出的函数值映射到 [0, _bitSize] 区间内
                var redisValue = hashFunc(value) % (ulong) _bitSize;

                var result = await _redisCache.SetBit(key, (long) redisValue, true);

                hasValue = hasValue && result;
            }

            return !hasValue;
        }
    }
}
