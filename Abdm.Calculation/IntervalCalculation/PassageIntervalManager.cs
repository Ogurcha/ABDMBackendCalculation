using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abdm.Calculation.DAL;
using Abdm.Calculation.Models;

namespace Abdm.Calculation.IntervalCalculation
{
    public class PassageIntervalManager(IPassageIntervalRepository passageIntervalRepository) : IPassageIntervalManager
    {
        /// <summary>
        /// Возвращает данные для расщета интервалов для данного иссо
        /// </summary>
        public async Task<PassageInterval[]> GetPassageIntervals(long issoId)
        {
            var passageIntervals = await passageIntervalRepository.GetPassageIntervals(issoId);

            foreach (var passageInterval in passageIntervals)
            {
                passageInterval.SafeInterval = [
                    passageInterval.SafetyLineLeft > 0.25 ? passageInterval.SafetyLineLeft : 0.25 + passageInterval.SafetyLineLeft,
                    passageInterval.SafetyLineRight > 0.25 ? passageInterval.TotalWidth - passageInterval.SafetyLineRight : passageInterval.TotalWidth - passageInterval.SafetyLineRight - 0.25
                ];
            }

            return passageIntervals;
        }

        /// <summary>
        /// Добирает координаты для проверок с учётом размера тележек
        /// </summary>
        /// <param name="passageIntervals">Должно быть хотя бы одно значение</param>
        public double[] GetDistinctXsWithWheels(
            double[] distinctXs,
            PassageInterval[] passageIntervals,
            Axle[] axles,
            double ladingPassageWidth)
        {
            var result = new List<double>();

            var differentWheelsWidths = axles.SelectMany(axle => axle.Wheels)
                .Distinct().Select(a => a / 2).ToArray();

            double minVal = double.NaN;
            double maxVal = double.NaN;
            foreach (var passageInterval in passageIntervals)
            {
                var low = passageInterval.SafeInterval[0] + ladingPassageWidth / 2;
                var high = passageInterval.SafeInterval[1] - ladingPassageWidth / 2;
                result.Add(low);
                result.Add(high);
                if (!(minVal < low))
                {
                    minVal = low;
                }

                if (!(maxVal > high))
                {
                    maxVal = high;
                }
            }
            foreach (var x in distinctXs)
            {
                if (minVal < x && x < maxVal)
                {
                    result.Add(x);
                }

                result.AddRange(differentWheelsWidths.Select(w => x + w).Where(x => minVal < x && x < maxVal));
                result.AddRange(differentWheelsWidths.Select(w => x - w).Where(x => minVal < x && x < maxVal));
            }

            return result.Order().Distinct().ToArray();
        }
    }
}
