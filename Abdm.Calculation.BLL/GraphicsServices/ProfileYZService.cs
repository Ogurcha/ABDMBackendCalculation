using System.Numerics;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Services;
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
        public double GetStrain(ProfileYZ profileVectors, double Y, PassTypeCalculationParameters message)
        {
            var surfaceMinY = message.Surface.MinY - message.Roadway.RoadHeight;
            var surfaceMaxY = message.Surface.MaxY + message.Roadway.RoadHeight;

            return message.LoadSchema.Axles
                .Where(a => a.WheelsDistance?.Length > 0)
                .Sum(a =>
                {
                    double axleY = Y + a.AbsolutePosition;
                    double weight = a.WheelWeight;
                    return (axleY >= surfaceMinY && axleY <= surfaceMaxY)
                        ? weight * profileVectors.GetZValueByY(axleY)
                        : 0d;
                });
        }

        public IEnumerable<Vector2> GetFloatYZFromProfile(ProfileYZ profileVectors)
        {
            foreach (var v in profileVectors.Vectors)
            {
                yield return new Vector2((float)v.Value.y, (float)v.Value.z);
            }
        }

        public double GetMaxZPosition(ProfileYZ profileVectors)
        {
            return profileVectors.Vectors.Values.OrderBy(v => v.z).Last().y;

        }
    }
}
