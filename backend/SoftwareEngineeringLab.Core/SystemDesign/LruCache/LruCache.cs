namespace SoftwareEngineeringLab.Core.SystemDesign.LruCache;

public record CacheEntry<TKey, TValue>(TKey Key, TValue Value);

/// <summary>
/// Least Recently Used (LRU) Cache using a Hash Map + Doubly Linked List.
/// Get: O(1)
/// Put: O(1)
/// </summary>
public class LruCache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, LinkedListNode<CacheEntry<TKey, TValue>>> _cache;
    private readonly LinkedList<CacheEntry<TKey, TValue>> _lruList;
    private readonly object _syncRoot = new();

    public int Capacity => _capacity;
    public int Count
    {
        get
        {
            lock (_syncRoot) return _cache.Count;
        }
    }

    public LruCache(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");

        _capacity = capacity;
        _cache = new Dictionary<TKey, LinkedListNode<CacheEntry<TKey, TValue>>>(capacity);
        _lruList = new LinkedList<CacheEntry<TKey, TValue>>();
    }

    public bool TryGet(TKey key, out TValue? value)
    {
        lock (_syncRoot)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                // Promoted to Most Recently Used (front)
                _lruList.Remove(node);
                _lruList.AddFirst(node);
                value = node.Value.Value;
                return true;
            }

            value = default;
            return false;
        }
    }

    public TValue? Get(TKey key)
    {
        if (TryGet(key, out var val))
            return val;

        throw new KeyNotFoundException($"Key '{key}' was not found in LRU Cache.");
    }

    public void Put(TKey key, TValue value)
    {
        lock (_syncRoot)
        {
            if (_cache.TryGetValue(key, out var existingNode))
            {
                // Update and promote to front
                _lruList.Remove(existingNode);
                var updatedNode = _lruList.AddFirst(new CacheEntry<TKey, TValue>(key, value));
                _cache[key] = updatedNode;
            }
            else
            {
                if (_cache.Count >= _capacity)
                {
                    // Evict Least Recently Used (tail)
                    var lru = _lruList.Last;
                    if (lru != null)
                    {
                        _cache.Remove(lru.Value.Key);
                        _lruList.RemoveLast();
                    }
                }

                var newNode = _lruList.AddFirst(new CacheEntry<TKey, TValue>(key, value));
                _cache[key] = newNode;
            }
        }
    }

    /// <summary>
    /// Returns the current cache state ordered from Most Recently Used to Least Recently Used.
    /// </summary>
    public List<CacheEntry<TKey, TValue>> GetOrderedEntries()
    {
        lock (_syncRoot)
        {
            return new List<CacheEntry<TKey, TValue>>(_lruList);
        }
    }
}
