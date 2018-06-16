namespace BloomFilter
{
    public class RedisCacheOptions
    {
        public string Server { get; set; }

        public string InstanceName { get; set; } = string.Empty;

        public int Database { get; set; } = -1;
    }
}
