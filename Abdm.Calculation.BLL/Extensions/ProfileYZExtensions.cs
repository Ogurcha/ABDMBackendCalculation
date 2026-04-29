using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Extensions
{
    public static class ProfileYZExtensions
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
            return profile.Vectors.Values;
        }

        public static double GetZValueByY(this ProfileYZ profile, double pointY)
        {
            (Vector2D v1, Vector2D v2) = Formulas.FindBetweenValues(profile.Vectors, pointY);
            return Formulas.GetOrdinat(v1, v2, pointY);
        }
    }
}