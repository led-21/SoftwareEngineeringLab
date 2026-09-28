using System.Security.Cryptography;
using System.Text;

namespace SoftwareEngineeringLab.Core.SystemDesign.ConsistentHashing;

public record NodeLocation(uint Hash, string NodeName, int ReplicaIndex);

/// <summary>
/// Consistent Hashing Ring with virtual nodes (replicas) to ensure uniform key distribution.
/// </summary>
public class ConsistentHashRing
{
    private readonly int _virtualNodesPerServer;
    private readonly SortedDictionary<uint, string> _ring = new();
    private readonly List<uint> _sortedKeys = new();
    private readonly object _syncRoot = new();

    public int VirtualNodesPerServer => _virtualNodesPerServer;
    public int TotalRingSlots => _ring.Count;

    public ConsistentHashRing(int virtualNodesPerServer = 50)
    {
        if (virtualNodesPerServer <= 0)
            throw new ArgumentOutOfRangeException(nameof(virtualNodesPerServer));

        _virtualNodesPerServer = virtualNodesPerServer;
    }

    public void AddNode(string serverNode)
    {
        if (string.IsNullOrWhiteSpace(serverNode))
            return;

        lock (_syncRoot)
        {
            for (int i = 0; i < _virtualNodesPerServer; i++)
            {
                uint hash = ComputeHash($"{serverNode}#replica#{i}");
                _ring[hash] = serverNode;
            }
            RebuildSortedKeys();
        }
    }

    public void RemoveNode(string serverNode)
    {
        if (string.IsNullOrWhiteSpace(serverNode))
            return;

        lock (_syncRoot)
        {
            for (int i = 0; i < _virtualNodesPerServer; i++)
            {
                uint hash = ComputeHash($"{serverNode}#replica#{i}");
                _ring.Remove(hash);
            }
            RebuildSortedKeys();
        }
    }

    /// <summary>
    /// Finds the designated server node clockwise on the hash ring.
    /// Uses Binary Search: O(log M) where M is total virtual nodes.
    /// </summary>
    public string? GetNode(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return null;

        lock (_syncRoot)
        {
            if (_sortedKeys.Count == 0)
                return null;

            uint keyHash = ComputeHash(key);

            // Binary search for first node with hash >= keyHash
            int index = _sortedKeys.BinarySearch(keyHash);

            if (index < 0)
            {
                index = ~index; // Bitwise complement returns next larger element index
            }

            // If key is beyond the largest hash on the ring, wrap around to first element (0)
            if (index >= _sortedKeys.Count)
            {
                index = 0;
            }

            uint selectedHash = _sortedKeys[index];
            return _ring[selectedHash];
        }
    }

    public List<NodeLocation> GetRingSnapshot()
    {
        lock (_syncRoot)
        {
            var list = new List<NodeLocation>(_ring.Count);
            foreach (var kvp in _ring)
            {
                list.Add(new NodeLocation(kvp.Key, kvp.Value, 0));
            }
            return list;
        }
    }

    private void RebuildSortedKeys()
    {
        _sortedKeys.Clear();
        _sortedKeys.AddRange(_ring.Keys);
    }

    private static uint ComputeHash(string input)
    {
        // FNV-1a 32-bit hash algorithm for fast uniform distribution
        uint hash = 2166136261;
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        foreach (byte b in bytes)
        {
            hash = (hash ^ b) * 16777619;
        }
        return hash;
    }
}
