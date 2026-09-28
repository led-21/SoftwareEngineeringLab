namespace SoftwareEngineeringLab.Core.DesignPatterns.Creational;

public interface IPaymentGateway
{
    string ProviderName { get; }
    string ProcessPayment(decimal amount);
}

public class CreditCardGateway : IPaymentGateway
{
    public string ProviderName => "CreditCard";
    public string ProcessPayment(decimal amount) => $"Processed ${amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)} via Credit Card Gateway.";
}

public class PixGateway : IPaymentGateway
{
    public string ProviderName => "Pix";
    public string ProcessPayment(decimal amount) => $"Generated instant Pix QR code for ${amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}.";
}

public class PayPalGateway : IPaymentGateway
{
    public string ProviderName => "PayPal";
    public string ProcessPayment(decimal amount) => $"Redirected to PayPal checkout for ${amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}.";
}

/// <summary>
/// Factory Pattern: Creates objects without exposing the instantiation logic to the client.
/// </summary>
public class PaymentGatewayFactory
{
    public IPaymentGateway Create(string type)
    {
        return type.ToLowerInvariant() switch
        {
            "creditcard" or "credit_card" or "a" => new CreditCardGateway(),
            "pix" or "b" => new PixGateway(),
            "paypal" or "c" => new PayPalGateway(),
            _ => throw new ArgumentException($"Unsupported payment provider: '{type}'.", nameof(type))
        };
    }
}
