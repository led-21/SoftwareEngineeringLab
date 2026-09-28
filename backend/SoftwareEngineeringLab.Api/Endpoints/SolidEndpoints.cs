namespace SoftwareEngineeringLab.Api.Endpoints;

public record SolidPrincipleDto(
    string Letter,
    string Name,
    string Summary,
    string ProblemDescription,
    string BadCodeSnippet,
    string GoodCodeSnippet,
    string KeyTakeaway
);

public static class SolidEndpoints
{
    private static readonly List<SolidPrincipleDto> Principles = new()
    {
        new(
            "S",
            "Single Responsibility Principle (SRP)",
            "A class should have one, and only one, reason to change.",
            "UserManager was managing both business creation logic and sending email notifications. Any change in email templates, SMTP configuration, or user entity schema forced modifications to the same class.",
            @"public class UserManagerBad {
    public void CreateUser(string username, string email) {
        // Business persistence
        Console.WriteLine($""User {username} created"");
        // Direct emailing violation
        SendEmail(email, ""Welcome!"");
    }
}",
            @"public class UserService {
    private readonly IUserRepository _repo;
    private readonly IEmailService _email;

    public UserService(IUserRepository repo, IEmailService email) {
        _repo = repo;
        _email = email;
    }

    public void RegisterUser(User user) {
        _repo.Save(user);
        _email.SendWelcome(user.Email);
    }
}",
            "Separate concerns into focused classes. High cohesion leads to easier unit testing and reduced ripple effects during maintenance."
        ),
        new(
            "O",
            "Open-Closed Principle (OCP)",
            "Software entities should be open for extension, but closed for modification.",
            "AreaCalculator relied on type checks (if shape is Rectangle ... else if shape is Circle). Adding a Triangle required opening and editing the existing AreaCalculator class, risking regressions.",
            @"public class AreaCalculatorBad {
    public double CalculateArea(object shape) {
        if (shape is Rectangle r) return r.Width * r.Height;
        if (shape is Circle c) return Math.PI * c.Radius * c.Radius;
        // Adding Triangle forces modifying this class!
        return 0;
    }
}",
            @"public interface IShape {
    double CalculateArea();
}

public class Rectangle : IShape {
    public double CalculateArea() => Width * Height;
}

public class Circle : IShape {
    public double CalculateArea() => Math.PI * Radius * Radius;
}

public class Triangle : IShape {
    public double CalculateArea() => 0.5 * Base * Height;
}",
            "Rely on polymorphism and abstractions. Introduce new behaviors by adding new classes without altering existing, tested code."
        ),
        new(
            "L",
            "Liskov Substitution Principle (LSP)",
            "Subtypes must be substitutable for their base types without altering the correctness of the program.",
            "Making Square inherit from Rectangle breaks the geometry invariant: setting the Width of a Rectangle unexpectedly changes its Height when the instance is a Square.",
            @"public class Rectangle {
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
}

public class Square : Rectangle {
    public override int Width {
        set { base.Width = value; base.Height = value; }
    }
}",
            @"public interface IGeometricShape {
    int Area();
}

public class Rectangle : IGeometricShape {
    public int Width { get; }
    public int Height { get; }
    public int Area() => Width * Height;
}

public class Square : IGeometricShape {
    public int Side { get; }
    public int Area() => Side * Side;
}",
            "Inheritance models 'behavioral compatibility', not just linguistic 'is-a' intuition. Prefer interfaces or composition when invariants differ."
        ),
        new(
            "I",
            "Interface Segregation Principle (ISP)",
            "Clients should not be forced to depend on interfaces they do not use.",
            "A monolithic IWorker interface forced a RobotWorker class to implement Eat() and Sleep() methods, resulting in NotImplementedExceptions.",
            @"public interface IWorkerFat {
    void Work();
    void Eat();
    void Sleep();
}

public class RobotWorker : IWorkerFat {
    public void Work() { }
    public void Eat() => throw new NotImplementedException();
    public void Sleep() => throw new NotImplementedException();
}",
            @"public interface IWorkable { void Work(); }
public interface IEatable { void Eat(); }
public interface ISleepable { void Sleep(); }

public class HumanWorker : IWorkable, IEatable, ISleepable { ... }
public class RobotWorker : IWorkable { ... }",
            "Keep interfaces small, cohesive, and focused on specific caller roles rather than bloated general-purpose contracts."
        ),
        new(
            "D",
            "Dependency Inversion Principle (DIP)",
            "High-level modules should not depend on low-level modules. Both should depend on abstractions.",
            "NotificationService directly created an instance of SmtpClient concrete class. It was impossible to mock the email dispatcher in unit tests or swap it with SMS or Cloud push without editing the class.",
            @"public class NotificationServiceBad {
    private readonly SmtpClient _smtp = new();

    public void Notify(string msg) {
        _smtp.Send(msg);
    }
}",
            @"public interface IMessageSender {
    string Send(string to, string message);
}

public class NotificationService {
    private readonly IMessageSender _sender;

    public NotificationService(IMessageSender sender) {
        _sender = sender;
    }
}",
            "Depend on abstractions (interfaces) injected from the outside (Dependency Injection). This unlocks modularity and clean testability."
        )
    };

    public static void MapSolidEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/solid", () => Results.Ok(Principles))
            .WithTags("SOLID");
    }
}
