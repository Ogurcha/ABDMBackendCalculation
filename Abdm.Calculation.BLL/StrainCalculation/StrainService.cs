using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Services;
using Abdm.Calculation.Graphics.Extensions;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.StrainCalculation
{
    public class StrainService : IStrainService
    {
        /// <summary>
        /// Рассчет напряжения на профиле с учётом тележек
        /// </summary>
        /// <param name="X"></param>
        public double GetStrain(PassTypeCalculationParameters message, ProfileYZ profileVectors, double Y)
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
    }
}
