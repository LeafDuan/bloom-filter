using System.IO;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace BloomFilter.Test
{
    public class RedisBloomFilterTest
    {
        private readonly ITestOutputHelper _output;

        public RedisBloomFilterTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public async Task TestRedisSetBit()
        {
            var redis = new RedisCache(new RedisCacheOptions
            {
                InstanceName = "leaf:bf",
                Database = 2,
                Server = "127.0.0.1:6379,abortConnect=false,defaultDatabase=2"
            });

            await redis.KeyDelete("test-url");

            var value = await redis.SetBit("test-url", 1, true);

            Assert.False(value);

            value = await redis.SetBit("test-url", 1, true);

            Assert.True(value);

            value = await redis.SetBit("test-url", 1, true);

            Assert.True(value);

            await redis.KeyDelete("test-url");

            redis.Dispose();
        }

        [Fact]
        public async Task TestFilter()
        {
            var urls = await File.ReadAllLinesAsync("urls.txt", Encoding.UTF8);

            var redis = new RedisCache(new RedisCacheOptions
            {
                InstanceName = "leaf:bf",
                Database = 2,
                Server = "127.0.0.1:6379,abortConnect=false,defaultDatabase=2"
            });

            var bloom = new RedisBloomFilter(redis);
            var duplication = false;

            await redis.KeyDelete("test-url");

            foreach (var url in urls)
            {
                if (!await bloom.TryAdd("test-url", url))
                {
                    duplication = true;

                    _output.WriteLine(url);
                }
            }

            await redis.KeyDelete("test-url");

            redis.Dispose();

            Assert.False(duplication);
        }
    }
}
