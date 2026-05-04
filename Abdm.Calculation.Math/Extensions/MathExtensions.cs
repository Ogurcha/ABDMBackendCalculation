using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        /// <param name="sortedVectors">
        /// SortedList<double, Vector2d>, где Key — X, Value.Y — f(X).
        /// Список должен быть отсортирован по возрастанию Key.
        /// </param>
        /// <returns>Список всех экстремумов и индексы экстремумов, которые являются максимумами.</returns>
        public static (
            List<Vector2D> extremums, 
            List<int> maximums, 
            List<Interval> postivePieces,
            Dictionary<double, Interval> postivePiecesMap
            ) FindExtremumsAndPositives(IEnumerable<Vector2D> sortedVectors)
        {
            var extremums = new List<Vector2D>();
            var maximums = new List<int>();
            var positivePiecesMap = new Dictionary<double, Interval>();
            var positivePieces = new List<Interval>();

            var array = sortedVectors.ToArray();

            if (array.Length < 3)
            {
                return (extremums, maximums, positivePieces, positivePiecesMap);
            }

            bool insidePositiveRegion = false;
            int intervalStartIndex = -1;
            var extremumCounter = 0;
            var interval = new Interval();

            PositivePieceIteration(0);
            for (int i = 1; i < array.Length - 1; i++)
            {
                ExtremumIteration(i);
                PositivePieceIteration(i);
            }
            PositivePieceIteration(array.Length - 1);

            return (extremums, maximums, positivePieces, positivePiecesMap);

            void ExtremumIteration(int i)
            {
                double yPrev = array[i - 1].Y;
                double yCurr = array[i].Y;
                double yNext = array[i + 1].Y;

                bool isMax = yPrev < yCurr && yCurr > yNext;
                bool isMin = yPrev > yCurr && yCurr < yNext;

                if (isMax)
                {
                    extremums.Add(array[i]);
                    maximums.Add(extremumCounter++);
                }
                if (isMin)
                {
                    extremums.Add(array[i]);
                    extremumCounter++;
                }
            }

            void PositivePieceIteration(int i) {
                if (!insidePositiveRegion && array[i].Y > 0)
                {
                    insidePositiveRegion = true;
                    intervalStartIndex = i;
                    interval = new Interval();
                    positivePieces.Add(interval);
                }
                if (insidePositiveRegion)
                {
                    positivePiecesMap.Add(array[i].X, interval);
                }
                if (insidePositiveRegion && (i == array.Length - 1 || array[i].Y <= 0))
                {
                    insidePositiveRegion = false;
                    interval.Start = intervalStartIndex > 0
                       ? GetIntersectionWithY(array[intervalStartIndex - 1], array[intervalStartIndex])!.Value
                       : array[intervalStartIndex].X;
                    interval.End = i < array.Length - 1
                       ? GetIntersectionWithY(array[i - 1], array[i])!.Value
                       : array[i].X;
                }
            }
        }
    }
}
