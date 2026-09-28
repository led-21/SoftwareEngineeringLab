namespace SoftwareEngineeringLab.Core.DesignPatterns.Creational;

public class Car
{
    public string Engine { get; set; } = "Standard 4-Cylinder";
    public string Wheels { get; set; } = "16-inch Alloy";
    public string Body { get; set; } = "Sedan";
    public bool HasSunroof { get; set; }
    public bool HasGps { get; set; }

    public string Describe() =>
        $"Car: [Body={Body}, Engine={Engine}, Wheels={Wheels}, Sunroof={HasSunroof}, GPS={HasGps}]";
}

public interface ICarBuilder
{
    ICarBuilder SetEngine(string engine);
    ICarBuilder SetWheels(string wheels);
    ICarBuilder SetBody(string body);
    ICarBuilder WithSunroof();
    ICarBuilder WithGps();
    Car Build();
}

public class SportsCarBuilder : ICarBuilder
{
    private Car _car = new();

    public ICarBuilder SetEngine(string engine)
    {
        _car.Engine = engine;
        return this;
    }

    public ICarBuilder SetWheels(string wheels)
    {
        _car.Wheels = wheels;
        return this;
    }

    public ICarBuilder SetBody(string body)
    {
        _car.Body = body;
        return this;
    }

    public ICarBuilder WithSunroof()
    {
        _car.HasSunroof = true;
        return this;
    }

    public ICarBuilder WithGps()
    {
        _car.HasGps = true;
        return this;
    }

    public Car Build()
    {
        var built = _car;
        _car = new Car(); // reset for reuse
        return built;
    }
}

/// <summary>
/// Builder Pattern: Separates the construction of a complex object from its representation.
/// </summary>
public class CarDirector
{
    public Car ConstructSportsCar(ICarBuilder builder)
    {
        return builder
            .SetBody("Coupe")
            .SetEngine("Twin-Turbo V8")
            .SetWheels("19-inch Sport Michelin")
            .WithSunroof()
            .WithGps()
            .Build();
    }
}
