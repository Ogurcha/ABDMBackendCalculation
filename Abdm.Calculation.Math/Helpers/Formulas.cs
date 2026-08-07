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

        /// <summary>
        /// Поиск двух значений отсортированного списка, между которыми лежит <paramref name="targetKey"/> за log(n)
        /// </summary>
        public static (T Left, T Right) FindBetweenValues<TKey, T>(this IList<T> sorted, TKey targetKey, Func<T, TKey> keyFunc) where TKey : struct, IComparisonOperators<TKey, TKey, bool>
        {
            const int MinIndex = 0;
            int MaxIndex = sorted.Count - 1;

            var first = sorted[MinIndex];
            var last = sorted[MaxIndex];

            if (targetKey <= keyFunc(first))
                return (first, first);

            if (targetKey >= keyFunc(last))
                return (last, last);

            int leftIndex = MinIndex;
            int rightIndex = MaxIndex;

            while (rightIndex - leftIndex > 1)
            {
                int midIndex = (leftIndex + rightIndex) / 2;
                var midKey = keyFunc(sorted[midIndex]);

                if (midKey == targetKey)
                    return (sorted[midIndex], sorted[midIndex]);

                if (midKey > targetKey)
                    rightIndex = midIndex;
                else
                    leftIndex = midIndex;
            }

            return (sorted[leftIndex], sorted[rightIndex]);
        }

        public static bool IsOdd(int number)
        {
            return number % 2 != 0;
        }

        /// <summary>
        /// Возвращает значение на оси ординат по значению на оси абсцисс на линии, определённой двумя точками
        /// </summary>
        public static double GetOrdinat(Vector2D v1, Vector2D v2, double X)
            => v2.X == v1.X
            ? v1.Y
            : (X - v1.X) * (v2.Y - v1.Y) / (v2.X - v1.X) + v1.Y;


        public static double GetYValueByX(this SortedList<double, Vector2D> sorted, double X)
        {
            (Vector2D v1, Vector2D v2) = FindBetweenValues(sorted, X);
            return GetOrdinat(v1, v2, X);
        }

        /// <summary>
        /// Поиск двух значений отсортированного списка, между которыми лежит <paramref name="targetKey"/> за log(n)
        /// </summary>
        public static (int? Left, int? Right) FindBetweenIndexes<TKey, T>(this IList<T> sorted, 
            TKey targetKey, 
            Func<T, TKey> keyFunc, 
            IEqualityComparer<TKey> equalityComparer) where TKey : struct, IComparisonOperators<TKey, TKey, bool>
        {
            const int MinIndex = 0;
            int MaxIndex = sorted.Count - 1;

            var first = sorted[MinIndex];
            var last = sorted[MaxIndex];

            if (targetKey <= keyFunc(first) || equalityComparer.Equals(targetKey, keyFunc(first)))
                return (null, MinIndex);

            if (targetKey >= keyFunc(last) || equalityComparer.Equals(targetKey, keyFunc(last)))
                return (MaxIndex, null);

            int leftIndex = MinIndex;
            int rightIndex = MaxIndex;

            while (rightIndex - leftIndex > 1)
            {
                int midIndex = (leftIndex + rightIndex) / 2;
                var midKey = keyFunc(sorted[midIndex]);

                if (equalityComparer.Equals(midKey, targetKey) || midKey == targetKey)
                    return (midIndex, midIndex);

                if (midKey > targetKey)
                    rightIndex = midIndex;
                else
                    leftIndex = midIndex;
            }

            return (leftIndex, rightIndex);
        }
    }
}
