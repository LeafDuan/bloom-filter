using System;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace BloomFilter
{
    public class RedisCache : IRedisCache, IDisposable
    {
        private readonly RedisCacheOptions _options;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private ConnectionMultiplexer _conn;
        private IDatabase _cache;

        public string InstanceName => _options.InstanceName;

        private string RedisKey(string key) => string.IsNullOrWhiteSpace(InstanceName) ? key : $"{InstanceName}:{key}";

        public RedisCache(RedisCacheOptions options)
        {
            _options = options;
        }

        public async Task<bool> SetBit(string key, long offset, bool bit)
        {
            await ConnectAsync();

            return await _cache.StringSetBitAsync(RedisKey(key), offset, bit);
        }

        private async Task ConnectAsync()
        {
            if (_conn != null)
                return;

            await _semaphore.WaitAsync();

            try
            {
                if (_conn == null)
                {
                    _conn = await ConnectionMultiplexer.ConnectAsync(_options.Server);
                    _cache = _conn.GetDatabase(_options.Database);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> KeyDelete(string key)
        {
            await ConnectAsync();

            return await _cache.KeyDeleteAsync(RedisKey(key));
        }

        public void Dispose()
        {
            _semaphore?.Dispose();
            _conn?.Dispose();
        }
    }
}
