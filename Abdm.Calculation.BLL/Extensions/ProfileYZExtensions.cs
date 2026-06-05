using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Extensions
{
    public static class ProfileYZExtensions
    {
        /// <summary>
        /// Рассчет значения поверхности влияния на профиле
        /// </summary>
        /// <param name="positivePieces">позитивные отрезки профиля, на которых искали напряжение. 
        /// Может вернуть null'ы, если рассчёт происходил в отрицательной зоне профиля></param>
        public static double GetZValueByY(
            this ProfileYZ profile, 
            double Y, 
            out (Interval? i1, Interval? i2) positivePieces)
        {
            var betweenValues = Formulas.FindBetweenValues(profile.SortedVectors, Y, (v) => v.X);
            var z = Formulas.GetOrdinat(betweenValues.Left, betweenValues.Right, Y);
            positivePieces = (profile.PositivePieceMap.TryGetValue(betweenValues.Left.X, out Interval? i1) ? i1 : null,
                profile.PositivePieceMap.TryGetValue(betweenValues.Right.X, out Interval? i2) ? i2 : null);
            return z;
        }

        /// <summary>
        /// Рассчет значения поверхности влияния на профиле используя расчёты объёмов поверхности
        /// </summary>
        /// <param name="positivePieces">позитивные отрезки профиля, на которых искали напряжение. 
        /// Может вернуть null'ы, если рассчёт происходил в отрицательной зоне профиля></param>
        public static double GetZValueByYSlabVersion(
            this ProfileYZExtended profile,
            double Y,
            out (Interval? i1, Interval? i2) positivePieces)
        {
            var trapezoidAreaLeft = CalculateZAreaAroundY(profile.SortedVectorsLeft, Y, profile.FootprintLength / 2, out _);
            var trapezoidAreaRight = CalculateZAreaAroundY(profile.SortedVectorsRight, Y, profile.FootprintLength / 2, out _);
            var trapezoidAreaCenter = CalculateZAreaAroundY(profile.SortedVectors, Y, profile.FootprintLength / 2, out var indexesCenter);

            var volume1 = MathExtensions.FrustrumVolume(profile.FootprintWidth / 2, trapezoidAreaLeft, trapezoidAreaCenter);
            var volume2 = MathExtensions.FrustrumVolume(profile.FootprintWidth / 2, trapezoidAreaRight, trapezoidAreaCenter);

            positivePieces = 
                (profile.PositivePieceMap.TryGetValue(profile.SortedVectors[indexesCenter.indexLeft].X, out Interval? i1) ? i1 : null,
                profile.PositivePieceMap.TryGetValue(profile.SortedVectors[indexesCenter.indexRight].X, out Interval? i2) ? i2 : null);

            return (volume1 + volume2) / profile.FootprintWidth;
        }

        /// <summary>
        /// Предполагая, что на поверхность давит не единичный вектор - а полоска с определённой протяжённостью и центром. 
        /// Считает площадь под поверхностью, возникающую в результате давления
        /// </summary>
        public static double CalculateZAreaAroundY(Vector2D[] vectors, 
            double Y, 
            double radius, 
            out (int indexLeft, int indexRight) indexes)
        {
            var YStart = Y - radius;
            var YFinish = Y + radius;

            var betweenIndexes1 = Formulas.FindBetweenIndexes(vectors, YStart, (v) => v.X);
            var betweenIndexes2 = Formulas.FindBetweenIndexes(vectors, YFinish, (v) => v.X);

            var indexStart = betweenIndexes1.Left + 1 ?? 0;
            var indexFinish = betweenIndexes2.Right + 1 ?? vectors.Length;

            var trapezoidVectors = vectors
                .Skip(indexStart)
                .Take(indexFinish - indexStart);

            if (betweenIndexes1.Left != null && betweenIndexes1.Right != null)
            {
                var z1 = Formulas.GetOrdinat(vectors[betweenIndexes1.Left!.Value],
                vectors[betweenIndexes1.Right!.Value],
                YStart);
                var firstVector = new Vector2D(YStart, z1);
                trapezoidVectors = trapezoidVectors.Prepend(firstVector);
            }

            if (betweenIndexes2.Left != null && betweenIndexes2.Right != null) {
                var z2 = Formulas.GetOrdinat(vectors[betweenIndexes2.Left!.Value],
                    vectors[betweenIndexes2.Right!.Value],
                    YFinish);
                var lastVector = new Vector2D(YFinish, z2);
                trapezoidVectors = trapezoidVectors.Append(lastVector);
            }

            indexes = (indexStart, indexFinish);

            return MathExtensions.CalculateAreaUnderCurve(trapezoidVectors.ToArray());
        }
    }
}