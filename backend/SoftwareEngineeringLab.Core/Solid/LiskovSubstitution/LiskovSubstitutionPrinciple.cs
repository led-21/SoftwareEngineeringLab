namespace SoftwareEngineeringLab.Core.Solid.LiskovSubstitution;

// ==================== LISKOV SUBSTITUTION PRINCIPLE (LSP) ====================
// "Subtypes must be substitutable for their base types without altering program correctness."

public static class LiskovSubstitutionStudy
{
    // ❌ VIOLATION: Classic Rectangle-Square problem.
    // Square alters Rectangle's invariant: Setting Width changes Height as well.
    public class RectangleBad
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public int Area() => Width * Height;
    }

    public class SquareBad : RectangleBad
    {
        public override int Width
        {
            get => base.Width;
            set { base.Width = value; base.Height = value; }
        }

        public override int Height
        {
            get => base.Height;
            set { base.Width = value; base.Height = value; }
        }
    }

    // A method assuming standard Rectangle behavior fails when passed a SquareBad:
    // rect.Width = 4; rect.Height = 5; Assert.Equal(20, rect.Area()); // Fails! Area is 25.

    //  REFACTORED: Model shapes by shared capabilities rather than incorrect inheritance.

    public interface IGeometricShape
    {
        int Area();
    }

    public class Rectangle : IGeometricShape
    {
        public int Width { get; }
        public int Height { get; }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Area() => Width * Height;
    }

    public class Square : IGeometricShape
    {
        public int Side { get; }

        public Square(int side)
        {
            Side = side;
        }

        public int Area() => Side * Side;
    }
}
