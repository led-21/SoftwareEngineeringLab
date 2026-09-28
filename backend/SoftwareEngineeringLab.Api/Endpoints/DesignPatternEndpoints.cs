namespace SoftwareEngineeringLab.Api.Endpoints;

public record DesignPatternDto(
    string Name,
    string Category,
    string ProblemSolved,
    string MermaidDiagram,
    string CsharpExample,
    string WhenToUse,
    string WhenToAvoid
);

public static class DesignPatternEndpoints
{
    private static readonly List<DesignPatternDto> Patterns = new()
    {
        new(
            "Singleton",
            "Creational",
            "Guarantees that a class has only one instance while providing a global access point to this instance.",
            "classDiagram\n    class Singleton {\n        -Singleton _instance$\n        -Singleton()\n        +Instance$ Singleton\n        +ExecuteOperation(caller)\n    }",
            @"public sealed class Singleton {
    private static readonly Lazy<Singleton> _instance = new(() => new Singleton());
    private Singleton() { }
    public static Singleton Instance => _instance.Value;
}",
            "Shared thread-safe hardware drivers, central metrics registries, or thread pools.",
            "When used as a disguised global variable, introducing hidden state couplings that break unit test isolation."
        ),
        new(
            "Factory Method",
            "Creational",
            "Provides an interface for creating objects in a superclass, allowing subclasses or factory selectors to alter the type of objects that will be created.",
            "classDiagram\n    class IPaymentGateway {\n        <<interface>>\n        +ProcessPayment(amount)\n    }\n    class CreditCardGateway\n    class PixGateway\n    class PayPalGateway\n    class PaymentGatewayFactory {\n        +Create(type) IPaymentGateway\n    }\n    IPaymentGateway <|.. CreditCardGateway\n    IPaymentGateway <|.. PixGateway\n    IPaymentGateway <|.. PayPalGateway\n    PaymentGatewayFactory ..> IPaymentGateway",
            @"public class PaymentGatewayFactory {
    public IPaymentGateway Create(string type) => type switch {
        ""creditcard"" => new CreditCardGateway(),
        ""pix"" => new PixGateway(),
        ""paypal"" => new PayPalGateway(),
        _ => throw new ArgumentException()
    };
}",
            "When the exact types and dependencies of the objects the code will work with aren't known beforehand.",
            "When the creation logic is straightforward and object variants rarely change."
        ),
        new(
            "Builder",
            "Creational",
            "Separates the construction of a complex object from its representation so that the same construction process can create different representations.",
            "classDiagram\n    class ICarBuilder {\n        <<interface>>\n        +SetEngine(engine)\n        +SetWheels(wheels)\n        +WithSunroof()\n        +Build() Car\n    }\n    class SportsCarBuilder\n    class CarDirector\n    ICarBuilder <|.. SportsCarBuilder\n    CarDirector --> ICarBuilder",
            @"var car = new SportsCarBuilder()
    .SetEngine(""Twin-Turbo V8"")
    .SetWheels(""19-inch Sport"")
    .WithSunroof()
    .Build();",
            "Constructing complex compound objects with numerous optional configuration parameters (avoiding telescoping constructors).",
            "When the product is simple and has few immutable parameters."
        ),
        new(
            "Adapter",
            "Structural",
            "Allows objects with incompatible interfaces to collaborate by converting the interface of a class into another interface expected by clients.",
            "classDiagram\n    class IModernJsonLogger {\n        <<interface>>\n        +LogJson(msg, level)\n    }\n    class LegacyXmlLogger {\n        +LogXml(payload)\n    }\n    class XmlToJsonLoggerAdapter {\n        -LegacyXmlLogger _legacy\n        +LogJson(msg, level)\n    }\n    IModernJsonLogger <|.. XmlToJsonLoggerAdapter\n    XmlToJsonLoggerAdapter --> LegacyXmlLogger",
            @"public class XmlToJsonLoggerAdapter : IModernJsonLogger {
    private readonly LegacyXmlLogger _legacy;
    public XmlToJsonLoggerAdapter(LegacyXmlLogger legacy) => _legacy = legacy;
    public string LogJson(string msg, string level) => _legacy.LogXml($""<Level>{level}</Level><Msg>{msg}</Msg>"");
}",
            "Integrating legacy libraries or third-party SDKs without modifying their closed source code.",
            "When you have control over the source code and can refactor directly."
        ),
        new(
            "Decorator",
            "Structural",
            "Attaches additional responsibilities to an object dynamically without altering its structure or relying on subclass proliferation.",
            "classDiagram\n    class IDataService {\n        <<interface>>\n        +FetchData(query)\n    }\n    class DatabaseDataService\n    class CachingDataServiceDecorator {\n        -IDataService _inner\n    }\n    IDataService <|.. DatabaseDataService\n    IDataService <|.. CachingDataServiceDecorator\n    CachingDataServiceDecorator --> IDataService",
            @"public class CachingDataServiceDecorator : DataServiceDecorator {
    private readonly Dictionary<string, string> _cache = new();
    public CachingDataServiceDecorator(IDataService inner) : base(inner) { }
    public override string FetchData(string q) => _cache.TryGetValue(q, out var val) ? val : (_cache[q] = base.FetchData(q));
}",
            "Adding transparent cross-cutting concerns like caching, logging, rate limiting, or encryption to services.",
            "When the decorator chain becomes so deeply nested that debugging flow order becomes difficult."
        ),
        new(
            "Facade",
            "Structural",
            "Provides a simplified, high-level interface to an entire subsystem of complex classes or microservices.",
            "classDiagram\n    class OrderFulfillmentFacade {\n        -InventoryService _inv\n        -PaymentService _pay\n        -ShippingService _ship\n        +PlaceOrder(client, item, amount)\n    }\n    OrderFulfillmentFacade --> InventoryService\n    OrderFulfillmentFacade --> PaymentService\n    OrderFulfillmentFacade --> ShippingService",
            @"public class OrderFulfillmentFacade {
    public string PlaceOrder(string customerId, string itemId, decimal amount) {
        if (!_inventory.CheckStock(itemId)) return ""Out of stock"";
        if (!_payment.ChargeCard(customerId, amount)) return ""Payment error"";
        return _shipping.GenerateTrackingCode(customerId, itemId);
    }
}",
            "Structuring a subsystem into layers or providing an ergonomic entry point for mobile clients and public APIs.",
            "When the facade turns into an omnipotent 'God Object' coupled to everything in the project."
        ),
        new(
            "Observer",
            "Behavioral",
            "Defines a subscription mechanism to notify multiple objects about any events that happen to the object they're observing.",
            "classDiagram\n    class IStockMarket {\n        <<interface>>\n        +Subscribe(observer)\n        +NotifyAll()\n    }\n    class IStockObserver {\n        <<interface>>\n        +OnPriceChanged(symbol, price)\n    }\n    class StockMarket\n    class InvestorMobileApp\n    IStockMarket <|.. StockMarket\n    IStockObserver <|.. InvestorMobileApp\n    StockMarket o--> IStockObserver",
            @"var market = new StockMarket();
market.Subscribe(new InvestorMobileApp(""Alice""));
market.SetPrice(""MSFT"", 420.50m); // Alice is automatically alerted",
            "Event-driven architectures, pub/sub notification hubs, reactive state updates in UI frameworks.",
            "When notifications trigger cascading side-effects in an unpredictable sequence."
        ),
        new(
            "Strategy",
            "Behavioral",
            "Defines a family of algorithms, encapsulates each one, and makes them interchangeable at runtime.",
            "classDiagram\n    class IDiscountStrategy {\n        <<interface>>\n        +ApplyDiscount(price) decimal\n    }\n    class RegularCustomerStrategy\n    class PremiumCustomerStrategy\n    class BlackFridayStrategy\n    class CheckoutContext {\n        -IDiscountStrategy _strategy\n        +CalculateFinalPrice(price)\n    }\n    IDiscountStrategy <|.. RegularCustomerStrategy\n    IDiscountStrategy <|.. PremiumCustomerStrategy\n    IDiscountStrategy <|.. BlackFridayStrategy\n    CheckoutContext --> IDiscountStrategy",
            @"var checkout = new CheckoutContext(new RegularCustomerStrategy());
checkout.CalculateFinalPrice(100m); // $100
checkout.SetStrategy(new BlackFridayStrategy());
checkout.CalculateFinalPrice(100m); // $60",
            "When you have multiple variants of an algorithm (e.g. routing, sorting, payment methods) selected dynamically.",
            "When you only have one or two static algorithms that rarely change."
        ),
        new(
            "Command",
            "Behavioral",
            "Encapsulates a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.",
            "classDiagram\n    class ICommand {\n        <<interface>>\n        +Execute()\n        +Undo()\n    }\n    class AppendTextCommand\n    class CommandHistoryInvoker {\n        -Stack~ICommand~ _history\n        +ExecuteCommand(cmd)\n        +UndoLast()\n    }\n    ICommand <|.. AppendTextCommand\n    CommandHistoryInvoker o--> ICommand",
            @"var invoker = new CommandHistoryInvoker();
invoker.ExecuteCommand(new AppendTextCommand(doc, ""Hello""));
invoker.UndoLast(); // reverts back",
            "Implementing multi-level Undo/Redo, job queues, or deferred task execution.",
            "When simple direct method invocations are sufficient and undo/redo is not required."
        )
    };

    public static void MapDesignPatternEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/design-patterns", () => Results.Ok(Patterns))
            .WithTags("DesignPatterns");
    }
}
