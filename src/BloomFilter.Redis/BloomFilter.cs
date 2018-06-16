/*
 * reference https://github.com/kongtianyi/BloomFilterRedis
 */

using System;
using System.Threading.Tasks;

namespace BloomFilter.Redis
{
    public class BloomFilter
    {
        private readonly IRedisCache _redisCache;

        private static readonly Func<string, ulong>[] HashFuncs =
        {
            //GeneralHashFunctions.RSHash,
            //GeneralHashFunctions.JSHash,
            GeneralHashFunctions.PJWHash,
            GeneralHashFunctions.ELFHash
        };

        public BloomFilter(IRedisCache redisCache)
        {
            this._redisCache = redisCache;
        }

        public async Task<bool> Filter(string key, string value)
        {
            var notExists = true;

            foreach (var hashFunc in HashFuncs)
            {
                // 将 hash 函数得出的函数值映射到 [0, 2^32-1] 区间内
                var redisValue = hashFunc(value) % (80000); // 1L << 32

                var result = await _redisCache.SetBit(key, (long) redisValue, true);

                notExists = notExists && result;
            }

            return !notExists;
        }
    }
}
