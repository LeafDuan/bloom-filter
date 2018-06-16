# bloom-filter
bloom filter for .net core

# redis bitmap 

* offset range [0, 2^23-1], 所以计算出来的 hash 值要映射到此区间
* max offset = 2^23-1 时，使用最大内存 512MB
* 内存使用的多少，根据 offset 区间决定

# hash
* hash 算法的好坏，性能影响很大
* hash 算法的个数，冲突的概率不一样

# 总结
这里使用 8 个 hash 函数，1w urls，很久才能出来，完全是不能用的状态
