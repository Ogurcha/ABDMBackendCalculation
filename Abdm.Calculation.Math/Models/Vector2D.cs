namespace Abdm.Calculation.Maths.Models
{
    public struct Vector2D
    {
        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X; 

        public double Y;

        public static implicit operator Vector2D((double X, double Y) value)
        {
            return new Vector2D(value.X, value.Y);
        }

        public static implicit operator (double X, double Y)(Vector2D value)
        {
            return (value.X, value.Y);
        }
    }
}
