namespace SoftwareEngineeringLab.Core.DesignPatterns.Behavioral;

public interface IStockObserver
{
    string ObserverName { get; }
    void OnPriceChanged(string symbol, decimal newPrice);
}

public interface IStockMarket
{
    void Subscribe(IStockObserver observer);
    void Unsubscribe(IStockObserver observer);
    void SetPrice(string symbol, decimal price);
}

/// <summary>
/// Observer Pattern: Defines a one-to-many dependency so that when one object changes state,
/// all dependents are notified automatically.
/// </summary>
public class StockMarket : IStockMarket
{
    private readonly List<IStockObserver> _observers = new();
    private readonly Dictionary<string, decimal> _prices = new();

    public void Subscribe(IStockObserver observer)
    {
        if (observer != null && !_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Unsubscribe(IStockObserver observer)
    {
        if (observer != null)
            _observers.Remove(observer);
    }

    public void SetPrice(string symbol, decimal price)
    {
        _prices[symbol] = price;
        NotifyAll(symbol, price);
    }

    private void NotifyAll(string symbol, decimal price)
    {
        foreach (var observer in _observers)
        {
            observer.OnPriceChanged(symbol, price);
        }
    }
}

public class InvestorMobileApp : IStockObserver
{
    public string ObserverName { get; }
    public List<string> Notifications { get; } = new();

    public InvestorMobileApp(string investorName)
    {
        ObserverName = investorName;
    }

    public void OnPriceChanged(string symbol, decimal newPrice)
    {
        Notifications.Add($"[{ObserverName}] Alert: {symbol} is now ${newPrice.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}");
    }
}
