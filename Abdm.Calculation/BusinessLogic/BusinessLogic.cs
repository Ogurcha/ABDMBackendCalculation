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

        




    }
}
