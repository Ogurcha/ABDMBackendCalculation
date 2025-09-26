public record struct SurfacePoint(double X, double Y, double Z)
{
    public static implicit operator (double X, double Y, double Z)(SurfacePoint value)
    {
        return (value.X, value.Y, value.Z);
    }

    public static implicit operator SurfacePoint((double X, double Y, double Z) value)
    {
        return new SurfacePoint(value.X, value.Y, value.Z);
    }
}