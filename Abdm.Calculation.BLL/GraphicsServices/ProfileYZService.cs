using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.Parameters;
using Abdm.Calculation.BLL.Models.Primitives;
using Abdm.Calculation.Graphics.Extensions;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.StrainCalculation
{
    public class ProfileYZService : IProfileYZService
    {
        /// <summary>
        /// Рассчет напряжения на профиле с учётом тележек
        /// </summary>
        /// <param name="X"></param>
        public double GetStrain(ProfileYZ profileVectors, double Y, LoadSchema loadSchema)
        {
            return loadSchema.Axles
                .Where(a => a.WheelsDistance?.Length > 0)
                .Sum(a =>
                {
                    double axleY = Y + a.AbsolutePosition;
                    double weight = a.WheelWeight;
                    return weight * profileVectors.GetZValueByY(axleY);
                });
        }

        public IEnumerable<Vector2D> GetYZFromProfile(ProfileYZ profile)
        {
            foreach (var v in profile.Vectors)
            {
                yield return new Vector2D(v.Value.y, v.Value.z);
            }
        }

        /// <summary>
        /// Возвращает точку с максимальным напряжением. 
        /// Учитывет размер ТС и различие положений его колёс
        /// </summary>
        public double GetMaxZPosition(ProfileYZ profile)
        {
            return profile.Vectors.Values.OrderBy(v => v.z).Last().y;
        }
    }
}
