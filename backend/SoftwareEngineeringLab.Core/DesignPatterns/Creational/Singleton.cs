namespace SoftwareEngineeringLab.Core.DesignPatterns.Creational;

/// <summary>
/// Singleton Pattern: Ensures a class has only one instance and provides a global point of access to it.
/// Uses .NET's Lazy&lt;T&gt; for thread-safe, lazy initialization without explicit lock overhead.
/// </summary>
public sealed class Singleton
{
    private static readonly Lazy<Singleton> _instance = new(() => new Singleton());

    private Singleton()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public static Singleton Instance => _instance.Value;

    public DateTime CreatedAt { get; }

    public string ExecuteOperation(string caller)
    {
        return $"Singleton instance ({CreatedAt:HH:mm:ss.fff}) executed for: {caller}";
    }
}
