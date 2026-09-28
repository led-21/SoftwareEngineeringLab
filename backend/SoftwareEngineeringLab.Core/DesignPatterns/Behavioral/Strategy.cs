namespace SoftwareEngineeringLab.Core.DesignPatterns.Behavioral;

public interface IDiscountStrategy
{
    string StrategyName { get; }
    decimal ApplyDiscount(decimal originalPrice);
}

public class RegularCustomerStrategy : IDiscountStrategy
{
    public string StrategyName => "Regular (No Discount)";
    public decimal ApplyDiscount(decimal originalPrice) => originalPrice;
}

public class PremiumCustomerStrategy : IDiscountStrategy
{
    public string StrategyName => "Premium (15% Off)";
    public decimal ApplyDiscount(decimal originalPrice) => originalPrice * 0.85m;
}

public class BlackFridayStrategy : IDiscountStrategy
{
    public string StrategyName => "Black Friday (40% Off)";
    public decimal ApplyDiscount(decimal originalPrice) => originalPrice * 0.60m;
}

/// <summary>
/// Strategy Pattern: Defines a family of algorithms, encapsulates each one, and makes them interchangeable.
/// </summary>
public class CheckoutContext
{
    private IDiscountStrategy _strategy;

    public CheckoutContext(IDiscountStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public void SetStrategy(IDiscountStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public decimal CalculateFinalPrice(decimal amount)
    {
        return _strategy.ApplyDiscount(amount);
    }
}
