using System.Numerics;

namespace Abdm.Calculation.Maths.Extensions
{
    /// <summary>
    /// Здесь решаем классические проблемы плавающей запятой, когда: 
    /// double a = 0.1 + 0.2; //  0.30000000000004
    /// double b = 0.3; // 0.29999999999999
    /// </summary>
    public static partial class FloatingPointProblemExtensions
    {
        public const double doubleTolerance = 1e-14;

        public const float floatTolerance = 1e-7f;

        public static bool AreAlmostEqual<T>(T a, T b, T tolerance)
            where T : IFloatingPointIeee754<T>
        {
            if (T.IsNaN(a) || T.IsNaN(b)) return false;
            if (T.IsInfinity(a) || T.IsInfinity(b)) return a == b;

            return T.Abs(a - b) < tolerance;
        }
    }
}
