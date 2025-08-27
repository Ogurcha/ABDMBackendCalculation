using System;
using System.Collections.Generic;
using System.Linq;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Models;
using Abdm.Calculation.PassTypeCalculation.DTO;
using g4;

namespace Abdm.Calculation.NewFolder
{
    public class BusinessLogic
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="passageIntervals">Должно быть хотя бы одно значение</param>
        public void UpdateDistinctXsWithWheels(
            MeshData meshData,
            PTCRequestMessage message, 
            PassageInterval[] passageIntervals)
        {
            var result = new List<double>();

            var differentWheelsWidths = message.NagruzkaSchema.Axles.SelectMany(axle => axle.Wheels)
                .Distinct().Select(a => a / 2).ToArray();

            double minVal = Double.NaN;
            double maxVal = Double.NaN;
            foreach (var passageInterval in passageIntervals)
            {
                var low = passageInterval.SafeInterval[0] + message.NagruzkaSchema.Width / 2;
                var high = passageInterval.SafeInterval[1] - message.NagruzkaSchema.Width / 2;
                result.Add(low);
                result.Add(high);
                if (!(minVal < low))
                    minVal = low;
                if (!(maxVal > high))
                    maxVal = high;
            }
            foreach (var x in meshData.DistinctXs)
            {
                if (minVal < x && x < maxVal)
                    result.Add(x);
                result.AddRange(differentWheelsWidths.Select(w => x + w).Where(x => minVal < x && x < maxVal));
                result.AddRange(differentWheelsWidths.Select(w => x - w).Where(x => minVal < x && x < maxVal));
            }

            meshData.DistinctXsWithWheels = result.Order().Distinct().ToArray();
        }

        //ais7PcCalculateColonna
        //LSxema посчитана - приходит с питона
        //Sxema - её нет, но по идее её можно добавить в код, и по айдишнику получать
        //Double[] sList = PassConditionOperation1 не нужен
        //xList - по поинтам, которые приходят из питона
        //По каждому иксу считаются колонны
        //алгоритм написан выше

        //Считаем колонну по иксу
        //Почистали профили по иксу
        //Во входящих параметрах есть MainInterval 0 - 13.7
        //ищутся углы в начале и в конце профиля AngleAtYMin/AngleAtYMax
        //но это пока что пропустим

        /// <summary>
        /// Нахождение экстремумов по оси Z вдоль оси Y.
        /// Возвращает Y-координаты экстремумов
        /// </summary>
        public List<double> GetExtremeByZ(Vector3d[] vectors)
        {
            var result = new List<double>();
            if (vectors.Length < 3) return result;

            var plateStart = Double.NaN;
            double previousDZ = vectors[1].z - vectors[0].z;

            for (int i = 1; i < vectors.Length - 1; i++) { 
                double dZ = vectors[i + 1].z - vectors[i].z;
                if (previousDZ > 0 && dZ <= 0)
                {
                    if (dZ == 0)
                        plateStart = vectors[i].y;
                    else
                        result.Add(vectors[i].y);
                }
                
                if (previousDZ == 0 && dZ < 0 && !Double.IsNaN(plateStart))
                {
                    result.Add(vectors[i].y + plateStart / 2);
                    plateStart = Double.NaN;
                }
                previousDZ = dZ;
            }
            return result;
        }


        /// <summary>
        /// возвращает результат сглаживания по гауссу
        /// </summary>
        /// <param name="profileVectors"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public Vector3d[] GetGaussianPoints(Vector2d[] profileVectors, )
        {
            private double x;

            public Extreme(double x)
            {
                this.x = x;
            }
        }



    }
}
