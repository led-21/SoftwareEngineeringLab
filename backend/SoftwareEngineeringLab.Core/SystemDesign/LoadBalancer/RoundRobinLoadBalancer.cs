namespace SoftwareEngineeringLab.Core.SystemDesign.LoadBalancer;

/// <summary>
/// Thread-safe Round Robin Load Balancer implementation with dynamic node management.
/// </summary>
public class RoundRobinLoadBalancer
{
    private readonly List<string> _servers = new();
    private int _currentIndex;
    private readonly object _syncRoot = new();

    public IReadOnlyList<string> Servers
    {
        get
        {
            lock (_syncRoot) return new List<string>(_servers);
        }
    }

    public RoundRobinLoadBalancer(IEnumerable<string>? initialServers = null)
    {
        if (initialServers != null)
        {
            _servers.AddRange(initialServers);
        }
    }

    public string? GetNextServer()
    {
        lock (_syncRoot)
        {
            if (_servers.Count == 0)
                return null;

            var server = _servers[_currentIndex];
            _currentIndex = (_currentIndex + 1) % _servers.Count;
            return server;
        }
    }

    public void AddServer(string server)
    {
        if (string.IsNullOrWhiteSpace(server))
            return;

        lock (_syncRoot)
        {
            if (!_servers.Contains(server))
                _servers.Add(server);
        }
    }

    public bool RemoveServer(string server)
    {
        lock (_syncRoot)
        {
            bool removed = _servers.Remove(server);
            if (removed && _currentIndex >= _servers.Count)
            {
                _currentIndex = 0;
            }
            return removed;
        }
    }
}
