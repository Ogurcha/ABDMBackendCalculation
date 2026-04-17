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

        /// <summary>
        /// Находит все строгие локальные экстремумы функции, заданной отсортированным списком точек.
        /// Сложность: O(n), один проход по данным.
        /// </summary>
        /// <param name="sortedPoints">
        /// SortedList<double, Vector2d>, где Key — X, Value.Y — f(X).
        /// Список должен быть отсортирован по возрастанию Key.
        /// </param>
        /// <returns>Список всех экстремумов и индексы экстремумов, которые являются максимумами.</returns>
        public static (List<Vector2D> extremums, List<int> maximums) FindAllExtremums(IEnumerable<Vector2D> vectors)
        {
            var extremums = new List<Vector2D>();
            var maximums = new List<int>();

            var array = vectors.ToArray();

            if (array.Length < 3)
            {
                return (extremums, []);
            }

            for (int i = 1; i < array.Length - 1; i++)
            {
                double yPrev = array[i - 1].Y;
                double yCurr = array[i].Y;
                double yNext = array[i + 1].Y;

                bool isMax = yPrev < yCurr && yCurr > yNext;
                bool isMin = yPrev > yCurr && yCurr < yNext;

                var counter = 0;
                if (isMax)
                {
                    extremums.Add(array[i]);
                    maximums.Add(counter++);
                }
                if (isMin)
                {
                    extremums.Add(array[i]);
                    counter++;
                }
            }

            return (extremums, maximums);
        }
    }
}
