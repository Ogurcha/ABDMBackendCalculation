using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Extensions
{
    public static class ProfileYZExtrensions
    {
        /// <summary>
        /// Рассчет напряжения на профиле
        /// </summary>
        /// <param name="Y"></param>
        public static double GetStrain(this ProfileYZ profile, double Y, double wheelWeight)
        {
            return wheelWeight * profile.GetZValueByY(Y);
        }

        public static IEnumerable<Vector2D> GetYZ(this ProfileYZ profile)
        {
            foreach (var v in profile.Vectors)
            {
                yield return new Vector2D(v.Value.X, v.Value.Y);
            }
        }

        public static double GetZValueByY(this ProfileYZ profile, double pointY)
        {
            (Vector2D v1, Vector2D v2) = Formulas.FindBetweenValues(profile.Vectors, pointY);
            return Formulas.GetOrdinat(v1, v2, pointY);
        }
    }
}