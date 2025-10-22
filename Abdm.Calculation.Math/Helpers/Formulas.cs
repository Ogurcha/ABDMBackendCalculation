using System.Numerics;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.Maths.Helpers
{
    public static class Formulas
    {
        /// <summary>
        /// Площадь трапеции, принмая, что основание трапеции равно (<paramref name="v1"/>.X, 0) и (<paramref name="v2"/>.X, 0)
        /// </summary>
        public static double TrapezoidArea(Vector2D v1, Vector2D v2)
        {
            var width = v2.X - v1.X;

            var averageHeight = (v1.Y + v2.Y) / 2d;

            return width * averageHeight;
        }

        /// <summary>
        /// Поиск двух значений отсортированного списка, между которыми лежит <paramref name="targetKey"/> за log(n)
        /// </summary>
        public static (T? Left, T? Right) FindBetweenValues<TKey, T>(this SortedList<TKey, T> sorted, TKey targetKey) where TKey : struct, IComparisonOperators<TKey, TKey, bool>
        {
            if (sorted.Count == 0) return (default, default);

            var keys = sorted.Keys;
            var values = sorted.Values;

            const int MinIndex = 0;
            int MaxIndex = sorted.Count - 1;

            if (targetKey <= keys[MinIndex])
                return (values[MinIndex], values[MinIndex]);

            if (targetKey >= keys[MaxIndex])
                return (values[MaxIndex], values[MaxIndex]);

            int leftIndex = MinIndex;
            int rightIndex = MaxIndex;

            while (rightIndex - leftIndex > 1)
            {
                int midIndex = (leftIndex + rightIndex) / 2;
                var midKey = keys[midIndex];

                if (midKey == targetKey)
                    return (values[midIndex], values[midIndex]);

                if (midKey > targetKey)
                    rightIndex = midIndex;
                else
                    leftIndex = midIndex;
            }

            return (values[leftIndex], values[rightIndex]);
        }

        public static bool IsOdd(int number)
        {
            return number % 2 != 0;
        }
    }
}
