namespace SoftwareEngineeringLab.Core.Solid.OpenClosed;

// ==================== OPEN-CLOSED PRINCIPLE (OCP) ====================
// "Software entities should be open for extension, but closed for modification."

public static class OpenClosedStudy
{
    // ❌ VIOLATION: To support a new shape, we must modify existing calculator code with more conditionals.
    public class AreaCalculatorBad
    {
        public double CalculateArea(object shape)
        {
            if (shape is RectangleShape rect)
                return rect.Width * rect.Height;

            if (shape is CircleShape circle)
                return Math.PI * circle.Radius * circle.Radius;

            // Adding Triangle requires modifying this method!
            return 0;
        }
    }

    public record RectangleShape(double Width, double Height);
    public record CircleShape(double Radius);

    //  REFACTORED: Open for extension via polymorphism, closed for modification.

    public interface IShape
    {
        double CalculateArea();
    }

    public class Rectangle : IShape
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double CalculateArea() => Width * Height;
    }

    public class Circle : IShape
    {
        public double Radius { get; }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public double CalculateArea() => Math.PI * Radius * Radius;
    }

    public class Triangle : IShape
    {
        public double Base { get; }
        public double Height { get; }

        public Triangle(double @base, double height)
        {
            Base = @base;
            Height = height;
        }

        public double CalculateArea() => 0.5 * Base * Height;
    }

    public class AreaCalculator
    {
        public double TotalArea(IEnumerable<IShape> shapes) => shapes.Sum(s => s.CalculateArea());
    }
}
