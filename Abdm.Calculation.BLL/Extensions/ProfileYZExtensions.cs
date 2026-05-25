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
        public static double GetZValueByY(
            this ProfileYZ profile, 
            double Y, 
            out (Interval? i1, Interval? i2) positivePieces)
        {
            var betweenValues = Formulas.FindBetweenValues(profile.Vectors, Y);
            var z = Formulas.GetOrdinat(betweenValues.Left, betweenValues.Right, Y);
            positivePieces = (profile.PositivePieceMap.TryGetValue(betweenValues.Left.X, out Interval? i1) ? i1 : null,
                profile.PositivePieceMap.TryGetValue(betweenValues.Right.X, out Interval? i2) ? i2 : null);
            return z;
        }

        public static IEnumerable<Vector2D> GetYZ(this ProfileYZ profile) 
            => profile.Vectors.Values;
    }
}