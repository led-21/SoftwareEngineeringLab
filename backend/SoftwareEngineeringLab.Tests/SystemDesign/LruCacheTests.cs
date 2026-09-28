using SoftwareEngineeringLab.Core.SystemDesign.LruCache;
using Xunit;

namespace SoftwareEngineeringLab.Tests.SystemDesign;

public class LruCacheTests
{
    [Fact]
    public void PutAndGet_StoresAndRetrievesValues()
    {
        var cache = new LruCache<string, int>(2);
        cache.Put("a", 100);
        cache.Put("b", 200);

        Assert.True(cache.TryGet("a", out int valA));
        Assert.Equal(100, valA);

        Assert.True(cache.TryGet("b", out int valB));
        Assert.Equal(200, valB);
    }

    [Fact]
    public void Put_WhenCapacityExceeded_EvictsLeastRecentlyUsed()
    {
        var cache = new LruCache<string, int>(2);
        cache.Put("a", 1);
        cache.Put("b", 2);

        // Access "a" so that "b" becomes the least recently used
        cache.TryGet("a", out _);

        // Insert "c" -> should evict "b"
        cache.Put("c", 3);

        Assert.True(cache.TryGet("a", out _));
        Assert.True(cache.TryGet("c", out _));
        Assert.False(cache.TryGet("b", out _)); // "b" was evicted
    }

    [Fact]
    public void GetOrderedEntries_ReflectsMostRecentlyUsedToLeast()
    {
        var cache = new LruCache<string, int>(3);
        cache.Put("k1", 1);
        cache.Put("k2", 2);
        cache.Put("k3", 3);

        // Access k1 -> promoted to top
        cache.TryGet("k1", out _);

        var entries = cache.GetOrderedEntries();
        Assert.Equal("k1", entries[0].Key); // MRU
        Assert.Equal("k3", entries[1].Key);
        Assert.Equal("k2", entries[2].Key); // LRU
    }
}
