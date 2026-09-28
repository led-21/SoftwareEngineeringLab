using SoftwareEngineeringLab.Core.DesignPatterns.Behavioral;
using SoftwareEngineeringLab.Core.DesignPatterns.Creational;
using SoftwareEngineeringLab.Core.DesignPatterns.Structural;
using Xunit;

namespace SoftwareEngineeringLab.Tests.DesignPatterns;

public class DesignPatternsTests
{
    [Fact]
    public void Singleton_ReturnsSameInstanceAcrossCalls()
    {
        var instance1 = Singleton.Instance;
        var instance2 = Singleton.Instance;

        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void Factory_CreatesCorrectPaymentGateway()
    {
        var factory = new PaymentGatewayFactory();
        var pix = factory.Create("pix");
        var creditCard = factory.Create("creditcard");

        Assert.Equal("Pix", pix.ProviderName);
        Assert.Equal("CreditCard", creditCard.ProviderName);
    }

    [Fact]
    public void Builder_ConstructsConfiguredCar()
    {
        var builder = new SportsCarBuilder();
        var director = new CarDirector();

        var car = director.ConstructSportsCar(builder);

        Assert.Equal("Twin-Turbo V8", car.Engine);
        Assert.True(car.HasSunroof);
    }

    [Fact]
    public void Adapter_EnablesModernLoggingViaLegacyComponent()
    {
        var legacy = new LegacyXmlLogger();
        IModernJsonLogger adapter = new XmlToJsonLoggerAdapter(legacy);

        string output = adapter.LogJson("System rebooted", "INFO");
        Assert.Contains("<Payload><Level>INFO</Level><Message>System rebooted</Message></Payload>", output);
    }

    [Fact]
    public void Observer_NotifiesSubscribersOnStateChange()
    {
        var market = new StockMarket();
        var app = new InvestorMobileApp("Alice");

        market.Subscribe(app);
        market.SetPrice("MSFT", 420.50m);

        Assert.Single(app.Notifications);
        Assert.Contains("MSFT is now $420.50", app.Notifications[0]);
    }

    [Fact]
    public void Strategy_AppliesCorrectDiscountPerPolicy()
    {
        var context = new CheckoutContext(new RegularCustomerStrategy());
        Assert.Equal(100m, context.CalculateFinalPrice(100m));

        context.SetStrategy(new PremiumCustomerStrategy());
        Assert.Equal(85m, context.CalculateFinalPrice(100m));

        context.SetStrategy(new BlackFridayStrategy());
        Assert.Equal(60m, context.CalculateFinalPrice(100m));
    }

    [Fact]
    public void Command_ExecutesAndUndoesTextModification()
    {
        var doc = new TextEditorDocument();
        var invoker = new CommandHistoryInvoker();

        invoker.ExecuteCommand(new AppendTextCommand(doc, "Hello "));
        invoker.ExecuteCommand(new AppendTextCommand(doc, "World!"));

        Assert.Equal("Hello World!", doc.Content);

        invoker.UndoLast();
        Assert.Equal("Hello ", doc.Content);
    }
}
