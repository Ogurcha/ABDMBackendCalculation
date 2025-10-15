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

        public static (T?, T?) FindBetweenValuesOld<TKey, T>(
            this SortedList<TKey, T> sorted, 
            TKey targetKey, 
            TKey? leftStartingPoint = null,
            int? leftStartingPointIndex = null,
            TKey? rightStartingPoint = null,
            int? rightStartingPointIndex = null)
            where TKey : struct, IComparisonOperators<TKey, TKey, bool>
        {
            if (sorted.Count == 0)
            {
                return (default, default);
            }

            TKey left; int leftIndex;
            if (leftStartingPoint == null || leftStartingPointIndex == null)
            {
                left = sorted.First().Key; leftIndex = 0;
            }
            else
            {
                left = leftStartingPoint.Value; leftIndex = leftStartingPointIndex.Value;
            }

            TKey right; int rightIndex;
            if (rightStartingPoint == null || rightStartingPointIndex == null)
            {
                right = sorted.Last().Key; rightIndex = sorted.Count - 1;
            }
            else
            {
                right = rightStartingPoint.Value; rightIndex = rightStartingPointIndex.Value;
            }

            if (targetKey <= left)
            {
                return (sorted[left], sorted[left]);
            }

            if (targetKey >= right)
            {
                return (sorted[right], sorted[right]);
            }

            if (sorted.Count == 2)
            {
                return (sorted[left], sorted[right]);
            }

            var midIndex = (rightIndex - leftIndex) / 2 + leftIndex;
            var mid = sorted.Keys[midIndex];

            if (mid == targetKey)
            {
                return (sorted[mid], sorted[mid]);
            }
            if (mid > targetKey)
            {
                return FindBetweenValuesOld(sorted, targetKey, left, leftIndex, mid, midIndex);
            }
            else 
            {
                return FindBetweenValuesOld(sorted, targetKey, mid, midIndex, right, rightIndex);
            }
        }
    }
}
