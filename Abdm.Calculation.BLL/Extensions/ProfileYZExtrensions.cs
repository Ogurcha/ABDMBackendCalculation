using Abdm.Calculation.Graphics.Extensions;
using Abdm.Calculation.Graphics.Models;
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
                yield return new Vector2D(v.Value.y, v.Value.z);
            }
        }
    }
}