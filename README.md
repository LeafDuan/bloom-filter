# bloom-filter
bloom filter for .net core

## redis bloom filter

> 使用到 bitmap 来完成任务   
> 适合分布式情况 

* offset range [0, 2^23-1], 计算出来的 hash 值要映射到此区间
* max offset = 2^23-1 时，使用最大内存 512MB
* 内存使用的多少，由 offset 区间决定

## memory bloom filter

> 使用到  BitArray 来完成任务  
> 适合单机使用

* 使用 BitArray 做 bitmap

### hash

* hash 算法的个数，冲突的概率不一样
* 有 general hash functions，也有第三方实现的 murmurh3 

# 总结

> 使用 8 个 hash 函数，生成 8 个 bit position，1w urls

* general hash 与 murmurh3 对比, 都很快  
* memory 与 redis 对比，memory 快很多

**hash 运算很快，redis 写太慢，成了主要耗时的地方**

PS: 测试太简陋，没有多大参考价值，更准确需使用 benchmark 进行性能测试
