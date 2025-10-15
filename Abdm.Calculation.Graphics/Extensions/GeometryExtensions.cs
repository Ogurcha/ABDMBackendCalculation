using System.Numerics;
using Abdm.Calculation.Graphics.Models;
using g4;

namespace Abdm.Calculation.Graphics.Extensions
{
    public static class GeometryExtensions
    {
        /// <summary>
        /// возвращает ординату по значению на оси абсцисс
        /// </summary>
        public static double GetOrdinat(Vector2d v1, Vector2d v2, double X)
            => (X - v1.x) * (v2.y - v1.y) / (v2.x - v1.x) + v1.y;


        public static double GetZValueByY(this ProfileYZ profile, double pointY)
        {
            (Vector3d v1, Vector3d v2) = FindBetweenValues(profile.Vectors, pointY);
            return GetOrdinat(v1.yz, v2.yz, pointY);
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
    }
}
