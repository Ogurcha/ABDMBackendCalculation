using System;
using System.Collections.Generic;
using System.Linq;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Models;
using Abdm.Calculation.PassTypeCalculation.DTO;

namespace Abdm.Calculation.NewFolder
{
    public class BusinessLogic
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
           
            foreach (var axle in message.NagruzkaSchema.Axles)
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
