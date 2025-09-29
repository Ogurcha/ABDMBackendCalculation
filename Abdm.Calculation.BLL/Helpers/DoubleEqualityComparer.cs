using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Extensions;

namespace Abdm.Calculation.BLL.Helpers
{
    public class DoubleEqualityComparer : IEqualityComparer<double>
    {
        /// <summary>
        /// Преобразуем double в целое число, умножая на 1024 и округляя.
        /// Это даёт нам разумное распределение, сохраняя при этом относительный порядок.
        /// </summary>
        private const int Thousand = 2^10;

        public bool Equals(double x, double y)
        {
            return FloatingPointProblemExtensions.AreAlmostEqual(x, y, FloatingPointProblemExtensions.doubleTolerance);
        }

        public int GetHashCode([DisallowNull] double obj)
        {
            var scaledValue = (int)Math.Round(obj * Thousand);
            return scaledValue.GetHashCode();
        }
    }
}
