using Abdm.Calculation.BLL.Models;
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
        public static double GetZValue(this ProfileYZ profile, double Y, out (Interval? i1, Interval? i2) positivePieces)
        {
            var z = profile.GetZValueByY(Y, out (Vector2D v1, Vector2D v2) betweenValues);
            positivePieces = (profile.PositivePieceMap.TryGetValue(betweenValues.v1.X, out Interval? i1) ? i1 : null,
                profile.PositivePieceMap.TryGetValue(betweenValues.v2.X, out Interval? i2) ? i2 : null);
            return z;
        }

        public static IEnumerable<Vector2D> GetYZ(this ProfileYZ profile)
        {
            return profile.Vectors.Values;
        }

        public static double GetZValueByY(this ProfileYZ profile, double pointY, out (Vector2D v1, Vector2D v2) betweenValues)
        {
            betweenValues = Formulas.FindBetweenValues(profile.Vectors, pointY);
            return Formulas.GetOrdinat(betweenValues.v1, betweenValues.v2, pointY);
        }
    }
}