using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Extensions;
using Abdm.Calculation.Graphics.Models;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.StrainCalculation
{
    public class ProfileYZService : IProfileYZService
    {
        /// <summary>
        /// Рассчет напряжения на профиле
        /// </summary>
        /// <param name="X"></param>
        public double GetStrain(ProfileYZ profile, double Y, double wheelWeight)
        {
            return wheelWeight * profile.GetZValueByY(Y);
        }

        public IEnumerable<Vector2D> GetYZFromProfile(ProfileYZ profile)
        {
            foreach (var v in profile.Vectors)
            {
                yield return new Vector2D(v.Value.y, v.Value.z);
            }
        }
    }
}
