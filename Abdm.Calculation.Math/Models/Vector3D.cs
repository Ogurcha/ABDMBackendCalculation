namespace Abdm.Calculation.Maths.Models
{
    public struct Vector3D
    {
        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X;

        public double Y;

        public double Z;

        public static implicit operator Vector3D((double X, double Y, double Z) value)
        {
            return new Vector3D(value.X, value.Y, value.Z);
        }

        public static implicit operator (double X, double Y, double Z)(Vector3D value)
        {
            return (value.X, value.Y, value.Z);
        }
    }
}
