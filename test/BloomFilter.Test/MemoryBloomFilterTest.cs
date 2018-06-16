using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace BloomFilter.Test
{
    public class MemoryBloomFilterTest
    {
        private readonly ITestOutputHelper _output;

        public MemoryBloomFilterTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public async Task TestGeneralHash()
        {
            Func<string, ulong>[] hashFuncs =
            {
                GeneralHashFunctions.RSHash,
                GeneralHashFunctions.JSHash,
                GeneralHashFunctions.PJWHash,
                GeneralHashFunctions.ELFHash,
                GeneralHashFunctions.BKDRHash,
                GeneralHashFunctions.SDBMHash,
                GeneralHashFunctions.DJBHash,
                GeneralHashFunctions.DEKHash
            };

            await TestHash(hashFuncs);
        }

        [Fact]
        public async Task TestMmh3()
        {
            Func<string, ulong>[] hashFuncs =
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

            await TestHash(hashFuncs);
        }

        private async Task TestHash(IEnumerable<Func<string, ulong>> hashFuncs)
        {
            var bloom = new MemoryBloomFilter(hashFuncs, 5000000);

            var urls = await File.ReadAllLinesAsync("urls.txt", Encoding.UTF8);

            var duplication = false;


            foreach (var url in urls)
            {
                if (!bloom.TryAdd(url))
                {
                    duplication = true;

                    _output.WriteLine(url);
                }
            }

            Assert.False(duplication);
        }
    }
}
