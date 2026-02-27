using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.Maths.Extensions
{
    public static class MathExtensions
    {
        /// <summary>
        /// Возвращает позитивные интервалы у двумерной функции. 
        /// Кейсы с касательным с нулём не учитываются.
        /// </summary>
        public static IEnumerable<Vector2D> GetPositvePieces(IList<Vector2D> function)
        {
            if (function.FirstOrDefault() is Vector2D first)
            {
                bool insidePositiveRegion = false;
                int intervalStartIndex = -1;

                for (int i = 0; i < function.Count; i++)
                {
                    if (!insidePositiveRegion && function[i].Y > 0)
                    {
                        insidePositiveRegion = true;
                        intervalStartIndex = i;
                    }
                    else if (insidePositiveRegion && (i == function.Count - 1 || function[i].Y <= 0))
                    {
                        insidePositiveRegion = false;
                        yield return new Vector2D(
                            intervalStartIndex > 0 
                            ? GetIntersectionWithY(function[intervalStartIndex - 1], function[intervalStartIndex])!.Value
                            : function[intervalStartIndex].X
                            , i < function.Count - 1 
                            ? GetIntersectionWithY(function[i - 1], function[i])!.Value
                            : function[i].X);
                    }
                }
            }
        }

        public static double? GetIntersectionWithY(Vector2D start, Vector2D finish)
        {
            double deltaX = finish.X - start.X;

            if (deltaX == 0)
                return null;

            double xIntersect = start.X +
                             (0 - start.Y) * deltaX /
                             (finish.Y - start.Y);

            return xIntersect;
        }

        /// <summary>
        /// Рассчёт площади под графиком
        /// </summary>
        public static double CalculateAreaUnderCurve(IList<Vector2D> points)
        {
            double totalArea = default;

            if (points.Count < 2)
                return totalArea;

            for (int i = 0; i < points.Count - 1; i++)
            {
                var currentPoint = points[i];
                var nextPoint = points[i + 1];

                totalArea += Math.Max((double)default, Formulas.TrapezoidArea(currentPoint, nextPoint));
            }

            return totalArea;
        }

        public static T Max<T>(T first, T second) where T : IComparable<T>
        {
            return first.CompareTo(second) >= 0 ? first : second;
        }

        /// <summary>
        /// перевод в decimal с 2мя знаками после запятой
        /// </summary>
        public static decimal ToDecimal(double value)
        {
            return decimal.Round((decimal)value, 2);
        }
    }
}
