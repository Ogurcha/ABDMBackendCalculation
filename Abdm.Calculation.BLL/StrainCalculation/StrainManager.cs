using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Entities;
using Abdm.Calculation.Graphics.Extensions;

namespace Abdm.Calculation.BLL.StrainCalculation
{
    public class StrainManager : IStrainManager
    {
        /// <summary>
        /// Рассчет напряжения на профиле с учётом тележек
        /// </summary>
        /// <param name="X"></param>
        public double GetStrain(PTCRequestMessage message, SmoothPoints smoothpoints, double Y)
        {
            var result = 0d;
            var surfaceMinY = message.Surface.MinY - message.Roadway.RoadHeight;
            var surfaceMaxY = message.Surface.MaxY + message.Roadway.RoadHeight;

            foreach (var axle in message.LadingSchema.Axles)
            {
                var wheelWeight = axle.Weight / axle.Wheels.Length;
                var ay = Y + axle.AbsY;
                if (ay >= surfaceMinY && ay <= surfaceMaxY)
                {
                    foreach (var wheel in axle.Wheels)
                    {
                        var coeff = smoothpoints.GetZ(ay);
                        result += wheelWeight * coeff;
                    }
                }
            }

            return result;
        }
    }
}
