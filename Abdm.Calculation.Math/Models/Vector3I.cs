namespace Abdm.Calculation.Maths.Models
{
    public struct Vector3I
    {
        public Vector3I(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X;

        public int Y;

        public int Z;

        public static implicit operator Vector3I((int X, int Y, int Z) value)
        {
            return new Vector3I(value.X, value.Y, value.Z);
        }

        public static implicit operator (int X, int Y, int Z)(Vector3I value)
        {
            return (value.X, value.Y, value.Z);
        }
    }
}
