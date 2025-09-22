using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
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
        public double GetStrain(PassTypeCalculationParameters message, SmoothPoints smoothpoints, double Y)
        {
            var surfaceMinY = message.Surface.MinY - message.Roadway.RoadHeight;
            var surfaceMaxY = message.Surface.MaxY + message.Roadway.RoadHeight;

            return message.LadingSchema.Axles
                .Where(a => a.Wheels?.Length > 0)
                .Sum(a =>
                {
                    double axleY = Y + a.AbsolutY;
                    double weight = a.Weight / (a.Wheels?.Length ?? 1);
                    return (axleY >= surfaceMinY && axleY <= surfaceMaxY)
                        ? weight * smoothpoints.GetZ(axleY)
                        : 0d;
                });
        }
    }
}
