using System.Threading.Tasks;

namespace BloomFilter.Redis
{
    public interface IRedisCache
    {
        string InstanceName { get; }

        /// <summary>
        ///   Sets or clears the bit at offset in the string value stored at key. The bit is
        //    either set or cleared depending on value, which can be either 0 or 1. When key
        //    does not exist, a new string value is created.The string is grown to make sure
        //    it can hold a bit at offset.
        /// </summary>
        Task<bool> SetBit(string key, long offset, bool bit);

        Task<bool> KeyDelete(string key);
    }
}