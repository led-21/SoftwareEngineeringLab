namespace SoftwareEngineeringLab.Core.DesignPatterns.Structural;

public interface IDataService
{
    string FetchData(string query);
}

public class DatabaseDataService : IDataService
{
    public string FetchData(string query) => $"[Database Query Result for '{query}']";
}

public abstract class DataServiceDecorator : IDataService
{
    protected readonly IDataService Inner;

    protected DataServiceDecorator(IDataService inner)
    {
        Inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public virtual string FetchData(string query) => Inner.FetchData(query);
}

/// <summary>
/// Decorator Pattern: Dynamically attaches additional responsibilities to an object without subclass explosion.
/// </summary>
public class CachingDataServiceDecorator : DataServiceDecorator
{
    private readonly Dictionary<string, string> _cache = new();

    public CachingDataServiceDecorator(IDataService inner) : base(inner) { }

    public override string FetchData(string query)
    {
        if (_cache.TryGetValue(query, out var cached))
        {
            return $"[Cache Hit] -> {cached}";
        }

        var result = base.FetchData(query);
        _cache[query] = result;
        return $"[Cache Miss] -> {result}";
    }
}
