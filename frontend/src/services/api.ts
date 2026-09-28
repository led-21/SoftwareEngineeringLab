export interface BinarySearchStep {
  stepNumber: number;
  left: number;
  mid: number;
  right: number;
  midValue: number;
  action: string;
}

export interface BinarySearchResponse {
  input: number[];
  target: number;
  foundIndex: number;
  steps: BinarySearchStep[];
  timeComplexity: string;
  spaceComplexity: string;
}

export interface SortResponse {
  algorithm: string;
  original: number[];
  sorted: number[];
  timeComplexity: string;
  spaceComplexity: string;
}

export interface RateLimitResponse {
  allowed: boolean;
  tokensRequested: number;
  remainingTokens: number;
  statusCode: number;
}

export interface LruEntry {
  key: string;
  value: string;
}

export interface SolidPrinciple {
  letter: string;
  name: string;
  summary: string;
  problemDescription: string;
  badCodeSnippet: string;
  goodCodeSnippet: string;
  keyTakeaway: string;
}

export interface DesignPattern {
  name: string;
  category: string;
  problemSolved: string;
  mermaidDiagram: string;
  csharpExample: string;
  whenToUse: string;
  whenToAvoid: string;
}

const CANDIDATE_HOSTS = [
  'http://localhost:5000',
  'http://127.0.0.1:5000',
  '', // Relative /api via Vite proxy
  'http://localhost:5067',
  'http://127.0.0.1:5067',
];

let activeApiHost: string | null = null;

// Helper to check if backend is alive
export async function checkBackendHealth(): Promise<boolean> {
  if (activeApiHost !== null) {
    try {
      const probeUrl = activeApiHost === '' ? '/api/solid' : `${activeApiHost}/`;
      const res = await fetch(probeUrl, { signal: AbortSignal.timeout(800) });
      if (res.ok) return true;
    } catch {
      activeApiHost = null;
    }
  }

  for (const host of CANDIDATE_HOSTS) {
    try {
      const probeUrl = host === '' ? '/api/solid' : `${host}/`;
      const res = await fetch(probeUrl, { signal: AbortSignal.timeout(800) });
      if (res.ok) {
        activeApiHost = host;
        return true;
      }
    } catch {
      // try next candidate
    }
  }

  return false;
}

export function getApiEndpoint(path: string): string {
  const host = activeApiHost ?? 'http://localhost:5000';
  const cleanPath = path.startsWith('/') ? path : `/${path}`;
  return `${host}/api${cleanPath}`;
}

// 1. Binary Search
export async function executeBinarySearch(nums: number[], target: number): Promise<BinarySearchResponse> {
  try {
    const res = await fetch(getApiEndpoint('/algorithms/binary-search'), {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ nums, target }),
      signal: AbortSignal.timeout(2000),
    });
    if (res.ok) return await res.json();
  } catch {
    // Fallback client simulation
  }

  // Pure TypeScript local fallback
  const steps: BinarySearchStep[] = [];
  let left = 0;
  let right = nums.length - 1;
  let foundIndex = -1;
  let step = 0;

  while (left <= right) {
    step++;
    const mid = Math.floor(left + (right - left) / 2);
    const midVal = nums[mid];

    if (midVal === target) {
      steps.push({
        stepNumber: step,
        left,
        mid,
        right,
        midValue: midVal,
        action: `Target ${target} located at index ${mid}.`,
      });
      foundIndex = mid;
      break;
    } else if (midVal < target) {
      steps.push({
        stepNumber: step,
        left,
        mid,
        right,
        midValue: midVal,
        action: `${midVal} < ${target}. Discarding left range [${left}..${mid}].`,
      });
      left = mid + 1;
    } else {
      steps.push({
        stepNumber: step,
        left,
        mid,
        right,
        midValue: midVal,
        action: `${midVal} > ${target}. Discarding right range [${mid}..${right}].`,
      });
      right = mid - 1;
    }
  }

  if (foundIndex === -1) {
    steps.push({
      stepNumber: step + 1,
      left,
      mid: -1,
      right,
      midValue: -1,
      action: `Target ${target} is not in the array.`,
    });
  }

  return {
    input: nums,
    target,
    foundIndex,
    steps,
    timeComplexity: 'O(log n)',
    spaceComplexity: 'O(1)',
  };
}

// 2. Sorting
export async function executeSort(nums: number[], algorithm: string): Promise<SortResponse> {
  try {
    const res = await fetch(getApiEndpoint('/algorithms/sort'), {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ nums, algorithm }),
      signal: AbortSignal.timeout(2000),
    });
    if (res.ok) return await res.json();
  } catch {
    // Fallback
  }

  const sorted = [...nums].sort((a, b) => a - b);
  return {
    algorithm: algorithm.toUpperCase(),
    original: nums,
    sorted,
    timeComplexity: 'O(n log n)',
    spaceComplexity: algorithm === 'mergesort' ? 'O(n)' : 'O(log n)',
  };
}

// 3. Rate Limiter Consume
let clientTokens = 10;
let clientLastRefill = Date.now();

export async function consumeRateLimitToken(): Promise<RateLimitResponse> {
  try {
    const res = await fetch(getApiEndpoint('/system-design/rate-limiter/consume'), {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ tokens: 1 }),
      signal: AbortSignal.timeout(2000),
    });
    if (res.ok) return await res.json();
  } catch {
    // Local fallback
  }

  const now = Date.now();
  const elapsedSeconds = (now - clientLastRefill) / 1000;
  clientTokens = Math.min(10, clientTokens + elapsedSeconds * 2);
  clientLastRefill = now;

  if (clientTokens >= 1) {
    clientTokens -= 1;
    return {
      allowed: true,
      tokensRequested: 1,
      remainingTokens: Number(clientTokens.toFixed(1)),
      statusCode: 200,
    };
  }

  return {
    allowed: false,
    tokensRequested: 1,
    remainingTokens: Number(clientTokens.toFixed(1)),
    statusCode: 429,
  };
}

// 4. URL Shortener
const localUrlMap = new Map<string, string>();
const localCodeMap = new Map<string, string>();

export async function shortenUrl(url: string): Promise<{ shortCode: string; shortUrl: string }> {
  try {
    const res = await fetch(getApiEndpoint('/system-design/url-shortener/shorten'), {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ url }),
      signal: AbortSignal.timeout(2000),
    });
    if (res.ok) return await res.json();
  } catch {
    // Fallback
  }

  if (localUrlMap.has(url)) {
    const code = localUrlMap.get(url)!;
    return { shortCode: code, shortUrl: `https://sel.dev/${code}` };
  }

  const code = Math.random().toString(36).substring(2, 9);
  localUrlMap.set(url, code);
  localCodeMap.set(code, url);
  return { shortCode: code, shortUrl: `https://sel.dev/${code}` };
}

// 5. SOLID Catalog
export async function fetchSolidPrinciples(): Promise<SolidPrinciple[]> {
  try {
    const res = await fetch(getApiEndpoint('/solid'), { signal: AbortSignal.timeout(2000) });
    if (res.ok) return await res.json();
  } catch {
    // Fallback to static principles definition
  }

  return [
    {
      letter: 'S',
      name: 'Single Responsibility Principle (SRP)',
      summary: 'A class should have one, and only one, reason to change.',
      problemDescription: 'UserManager handled user database operations AND sent welcome emails directly, conflating business logic with notification protocols.',
      badCodeSnippet: `public class UserManagerBad {\n    public void CreateUser(string username, string email) {\n        Console.WriteLine($"User {username} created");\n        SendEmail(email, "Welcome!"); // SRP Violation\n    }\n}`,
      goodCodeSnippet: `public class UserService {\n    private readonly IUserRepository _repo;\n    private readonly IEmailService _email;\n    public void RegisterUser(User user) {\n        _repo.Save(user);\n        _email.SendWelcome(user.Email);\n    }\n}`,
      keyTakeaway: 'High cohesion ensures modifications to email logic do not risk breaking user persistence rules.',
    },
    {
      letter: 'O',
      name: 'Open-Closed Principle (OCP)',
      summary: 'Software entities should be open for extension, but closed for modification.',
      problemDescription: 'AreaCalculator checked concrete shape types with if-else chains. Adding Triangle required modifying existing tested code.',
      badCodeSnippet: `public class AreaCalculatorBad {\n    public double CalculateArea(object shape) {\n        if (shape is Rectangle r) return r.Width * r.Height;\n        if (shape is Circle c) return Math.PI * c.Radius * c.Radius;\n        return 0; // Violates OCP when adding new shapes\n    }\n}`,
      goodCodeSnippet: `public interface IShape {\n    double CalculateArea();\n}\npublic class Triangle : IShape {\n    public double CalculateArea() => 0.5 * Base * Height;\n}`,
      keyTakeaway: 'Polymorphic interfaces allow new behaviors to be introduced simply by writing new classes.',
    },
    {
      letter: 'L',
      name: 'Liskov Substitution Principle (LSP)',
      summary: 'Subtypes must be substitutable for their base types without altering correctness.',
      problemDescription: 'Inheriting Square from Rectangle causes setters to alter both Width and Height, breaking rectangular geometric invariants in client calculations.',
      badCodeSnippet: `public class SquareBad : Rectangle {\n    public override int Width {\n        set { base.Width = value; base.Height = value; } // Violates LSP\n    }\n}`,
      goodCodeSnippet: `public interface IGeometricShape {\n    int Area();\n}\npublic class Rectangle : IGeometricShape { ... }\npublic class Square : IGeometricShape { ... }`,
      keyTakeaway: 'Subtyping must preserve behavioral guarantees, not merely linguistic categorization.',
    },
    {
      letter: 'I',
      name: 'Interface Segregation Principle (ISP)',
      summary: 'Clients should not be forced to depend on interfaces they do not use.',
      problemDescription: 'A fat IWorker interface forced RobotWorker to throw NotImplementedExceptions for Eat() and Sleep().',
      badCodeSnippet: `public interface IWorkerFat {\n    void Work();\n    void Eat();\n    void Sleep();\n}\npublic class RobotWorker : IWorkerFat {\n    public void Eat() => throw new NotImplementedException();\n}`,
      goodCodeSnippet: `public interface IWorkable { void Work(); }\npublic interface IEatable { void Eat(); }\npublic class RobotWorker : IWorkable { void Work() { } }`,
      keyTakeaway: 'Small, role-specific interfaces prevent polluting consumers with useless method stubs.',
    },
    {
      letter: 'D',
      name: 'Dependency Inversion Principle (DIP)',
      summary: 'High-level modules should depend on abstractions, not concrete implementations.',
      problemDescription: 'NotificationService directly instantiated a concrete SmtpClient, making mock testing impossible.',
      badCodeSnippet: `public class NotificationServiceBad {\n    private readonly SmtpClient _smtp = new();\n    public void Alert(string msg) => _smtp.Send(msg);\n}`,
      goodCodeSnippet: `public class NotificationService {\n    private readonly IMessageSender _sender;\n    public NotificationService(IMessageSender sender) {\n        _sender = sender;\n    }\n}`,
      keyTakeaway: 'Invert control via dependency injection to keep components decoupled and trivially testable.',
    },
  ];
}

// 6. Design Patterns Catalog
export async function fetchDesignPatterns(): Promise<DesignPattern[]> {
  try {
    const res = await fetch(getApiEndpoint('/design-patterns'), { signal: AbortSignal.timeout(2000) });
    if (res.ok) return await res.json();
  } catch {
    // Fallback
  }

  return [
    {
      name: 'Singleton',
      category: 'Creational',
      problemSolved: 'Ensures a class has only one instance and provides a global access point.',
      mermaidDiagram: 'classDiagram\n    class Singleton {\n        -Singleton _instance$\n        -Singleton()\n        +Instance$ Singleton\n        +ExecuteOperation()\n    }',
      csharpExample: `public sealed class Singleton {\n    private static readonly Lazy<Singleton> _instance = new(() => new Singleton());\n    private Singleton() { }\n    public static Singleton Instance => _instance.Value;\n}`,
      whenToUse: 'Central logging registry, thread pool manager, hardware device connector.',
      whenToAvoid: 'When used as a disguised global state variable, complicating unit testing.',
    },
    {
      name: 'Factory Method',
      category: 'Creational',
      problemSolved: 'Delegates object creation to specialized factory methods or sub-classes.',
      mermaidDiagram: 'classDiagram\n    class IPaymentGateway {\n        <<interface>>\n        +ProcessPayment(amount)\n    }\n    class CreditCardGateway\n    class PixGateway\n    class PaymentGatewayFactory {\n        +Create(type) IPaymentGateway\n    }\n    IPaymentGateway <|.. CreditCardGateway\n    IPaymentGateway <|.. PixGateway\n    PaymentGatewayFactory ..> IPaymentGateway',
      csharpExample: `public class PaymentGatewayFactory {\n    public IPaymentGateway Create(string type) => type switch {\n        "creditcard" => new CreditCardGateway(),\n        "pix" => new PixGateway(),\n        _ => throw new ArgumentException()\n    };\n}`,
      whenToUse: 'When the exact classes to instantiate depend on dynamic runtime parameters or config.',
      whenToAvoid: 'When creating simple objects with fixed lifetimes and no polymorphic variants.',
    },
    {
      name: 'Builder',
      category: 'Creational',
      problemSolved: 'Separates complex object construction from its representation.',
      mermaidDiagram: 'classDiagram\n    class ICarBuilder {\n        <<interface>>\n        +SetEngine(e)\n        +WithSunroof()\n        +Build() Car\n    }\n    class SportsCarBuilder\n    class CarDirector\n    ICarBuilder <|.. SportsCarBuilder\n    CarDirector --> ICarBuilder',
      csharpExample: `var car = new SportsCarBuilder()\n    .SetEngine("V8 Twin-Turbo")\n    .WithSunroof()\n    .Build();`,
      whenToUse: 'Constructing composite objects with numerous optional flags or multi-step setup.',
      whenToAvoid: 'Simple objects with only 1 or 2 mandatory constructor arguments.',
    },
    {
      name: 'Adapter',
      category: 'Structural',
      problemSolved: 'Converts the interface of a class into another interface expected by clients.',
      mermaidDiagram: 'classDiagram\n    class IModernLogger {\n        <<interface>>\n        +LogJson(msg)\n    }\n    class LegacyXmlLogger {\n        +LogXml(payload)\n    }\n    class LoggerAdapter {\n        -LegacyXmlLogger _legacy\n    }\n    IModernLogger <|.. LoggerAdapter\n    LoggerAdapter --> LegacyXmlLogger',
      csharpExample: `public class XmlToJsonAdapter : IModernLogger {\n    private readonly LegacyXmlLogger _legacy;\n    public XmlToJsonAdapter(LegacyXmlLogger legacy) => _legacy = legacy;\n    public void LogJson(string msg) => _legacy.LogXml($"<msg>{msg}</msg>");\n}`,
      whenToUse: 'Reusing existing third-party or legacy classes whose interfaces do not match.',
      whenToAvoid: 'When you can directly modify the target class interface.',
    },
    {
      name: 'Decorator',
      category: 'Structural',
      problemSolved: 'Attaches additional responsibilities dynamically without altering class structure.',
      mermaidDiagram: 'classDiagram\n    class IDataService {\n        <<interface>>\n        +FetchData(q)\n    }\n    class DatabaseService\n    class CachingDecorator {\n        -IDataService _inner\n    }\n    IDataService <|.. DatabaseService\n    IDataService <|.. CachingDecorator\n    CachingDecorator --> IDataService',
      csharpExample: `public class CachingDecorator : DataServiceDecorator {\n    public override string Fetch(string q) => _cache.GetOrAdd(q, () => base.Fetch(q));\n}`,
      whenToUse: 'Cross-cutting concerns: caching, metrics instrumentation, encryption, access control.',
      whenToAvoid: 'When adding behaviors directly to small single-purpose classes is cleaner.',
    },
    {
      name: 'Facade',
      category: 'Structural',
      problemSolved: 'Provides a unified, simplified interface to an intricate set of subsystems.',
      mermaidDiagram: 'classDiagram\n    class OrderFacade {\n        -Inventory _inv\n        -Payment _pay\n        -Shipping _ship\n        +PlaceOrder()\n    }\n    OrderFacade --> Inventory\n    OrderFacade --> Payment\n    OrderFacade --> Shipping',
      csharpExample: `public class OrderFacade {\n    public string Checkout(string item, decimal amt) {\n        if (!_inv.HasStock(item)) return "Out of stock";\n        _pay.Charge(amt);\n        return _ship.CreateLabel(item);\n    }\n}`,
      whenToUse: 'Presenting a simple API for clients while hiding internal micro-library complexity.',
      whenToAvoid: 'When clients legitimately need low-level direct access to subsystem internals.',
    },
    {
      name: 'Observer',
      category: 'Behavioral',
      problemSolved: 'Defines a subscription mechanism to notify multiple listeners of state changes.',
      mermaidDiagram: 'classDiagram\n    class IStockMarket {\n        +Subscribe(obs)\n        +NotifyAll()\n    }\n    class IStockObserver {\n        +OnPriceChanged(symbol, price)\n    }\n    IStockMarket o--> IStockObserver',
      csharpExample: `market.Subscribe(new MobileAppObserver("Alice"));\nmarket.SetPrice("MSFT", 420.50m); // Alice alerted automatically`,
      whenToUse: 'Event-driven architectures, pub-sub messaging, reactive UI updates.',
      whenToAvoid: 'When notifications might trigger cascading cycles or memory leaks (lack of unsubscription).',
    },
    {
      name: 'Strategy',
      category: 'Behavioral',
      problemSolved: 'Defines a family of interchangeable algorithms selected at runtime.',
      mermaidDiagram: 'classDiagram\n    class IDiscountStrategy {\n        <<interface>>\n        +ApplyDiscount(p)\n    }\n    class RegularStrategy\n    class BlackFridayStrategy\n    class CheckoutContext {\n        -IDiscountStrategy _strategy\n    }\n    IDiscountStrategy <|.. RegularStrategy\n    IDiscountStrategy <|.. BlackFridayStrategy\n    CheckoutContext --> IDiscountStrategy',
      csharpExample: `var checkout = new CheckoutContext(new BlackFridayStrategy());\nvar total = checkout.CalculateFinalPrice(100m); // $60`,
      whenToUse: 'Dynamic payment gateways, sorting options, tax calculation rules per region.',
      whenToAvoid: 'When only one static algorithm is needed.',
    },
    {
      name: 'Command',
      category: 'Behavioral',
      problemSolved: 'Encapsulates a request as an object, enabling parameterization, queuing, and undo.',
      mermaidDiagram: 'classDiagram\n    class ICommand {\n        +Execute()\n        +Undo()\n    }\n    class AppendTextCommand\n    class CommandInvoker {\n        -Stack _history\n        +Execute(cmd)\n        +UndoLast()\n    }\n    ICommand <|.. AppendTextCommand\n    CommandInvoker o--> ICommand',
      csharpExample: `invoker.Execute(new AppendTextCommand(doc, "Hello"));\ninvoker.UndoLast(); // reverts back`,
      whenToUse: 'Multi-level Undo/Redo operations, transactional rollbacks, task job queues.',
      whenToAvoid: 'Simple method calls that never need to be queued or reversed.',
    },
  ];
}
