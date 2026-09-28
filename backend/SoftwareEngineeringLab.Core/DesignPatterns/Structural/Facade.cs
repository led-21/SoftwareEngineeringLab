namespace SoftwareEngineeringLab.Core.DesignPatterns.Structural;

public class InventoryService
{
    public bool CheckStock(string itemId) => true;
}

public class PaymentService
{
    public bool ChargeCard(string customerId, decimal amount) => amount > 0;
}

public class ShippingService
{
    public string GenerateTrackingCode(string customerId, string itemId) =>
        $"TRK-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";
}

/// <summary>
/// Facade Pattern: Provides a unified, high-level interface to a complex set of subsystem interfaces.
/// </summary>
public class OrderFulfillmentFacade
{
    private readonly InventoryService _inventory = new();
    private readonly PaymentService _payment = new();
    private readonly ShippingService _shipping = new();

    public string PlaceOrder(string customerId, string itemId, decimal amount)
    {
        if (!_inventory.CheckStock(itemId))
            return "Order Failed: Item out of stock.";

        if (!_payment.ChargeCard(customerId, amount))
            return "Order Failed: Payment declined.";

        var tracking = _shipping.GenerateTrackingCode(customerId, itemId);
        return $"Order Successful! Tracking Code: {tracking}";
    }
}
