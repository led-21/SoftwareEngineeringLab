# Design Patterns Catalog & Reference

This guide covers Creational, Structural, and Behavioral patterns implemented in `SoftwareEngineeringLab.Core.DesignPatterns`.

---

## 1. Creational Patterns

### Singleton Pattern
- **Intent**: Guarantee a single instance with a global point of access.
- **Modern .NET Idiom**: Use `System.Lazy<T>` for thread-safe, lazy, zero-overhead initialization.
- **Trade-off**: Caution against hidden global state that breaks test isolation.

### Factory Method Pattern
- **Intent**: Delegate object instantiation to a specialized creator.
- **Example**: `PaymentGatewayFactory` instantiating `PixGateway`, `CreditCardGateway`, or `PayPalGateway` based on input parameters.

### Builder Pattern
- **Intent**: Construct complex composite objects step-by-step.
- **Example**: `SportsCarBuilder` and `CarDirector` avoiding telescoping constructors with 10+ parameters.

---

## 2. Structural Patterns

### Adapter Pattern
- **Intent**: Bridge incompatible interfaces without altering their underlying code.
- **Example**: `XmlToJsonLoggerAdapter` wrapping legacy XML logging services for modern JSON log consumers.

### Decorator Pattern
- **Intent**: Dynamically attach cross-cutting responsibilities (caching, logging, metrics) to objects.
- **Example**: `CachingDataServiceDecorator` transparently caching database queries.

### Facade Pattern
- **Intent**: Provide a simplified high-level interface to a complex subsystem.
- **Example**: `OrderFulfillmentFacade` coordinating inventory checks, payment authorization, and shipping label generation.

---

## 3. Behavioral Patterns

### Observer Pattern
- **Intent**: Define a one-to-many subscription dependency where state changes automatically broadcast to observers.
- **Example**: `StockMarket` notifying multiple `InvestorMobileApp` instances upon price changes.

### Strategy Pattern
- **Intent**: Encapsulate interchangeable families of algorithms at runtime.
- **Example**: `CheckoutContext` switching dynamically between `RegularCustomerStrategy`, `PremiumCustomerStrategy`, and `BlackFridayStrategy`.

### Command Pattern
- **Intent**: Encapsulate a request as an object, supporting deferred execution, queues, and undo/redo stacks.
- **Example**: `TextEditorDocument` modified by `AppendTextCommand` and rolled back via `CommandHistoryInvoker.UndoLast()`.
