namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class ProfileVector
    {
        public ProfileVector(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public decimal X;

        public decimal Y;

        public static implicit operator ProfileVector((decimal X, decimal Y) value)
        {
            return new ProfileVector(value.X, value.Y);
        }

        public static implicit operator (decimal X, decimal Y)(ProfileVector value)
        {
            return (value.X, value.Y);
        }
    }
}
