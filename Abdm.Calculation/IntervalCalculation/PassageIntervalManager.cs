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
        /// <param name="distinctXs">Массив точек по оси Х для всей поверхности ИССО</param>
        /// <param name="passageInterval">Интервал проезда по оси Х, по которому должно проехать ТС</param>
        /// <param name="axles">Информация о Тележках транспортного средства</param>
        /// <param name="carWidth">Общие габариты ТС</param>
        /// <returns>Массив точек по оси Х внутри данного интервала, и с учётом заездов и с учётом размера колёс</returns>
        public double[] GetDistinctXsWithWheels(
            double[] distinctXs,
            PassageInterval passageInterval,
            Axle[] axles,
            double carWidth)
        {
            var result = new List<double>();

            var differentWheelsWidths = axles.SelectMany(axle => axle.Wheels)
                .Distinct().Select(a => a / 2).ToArray();

            var low = passageInterval.SafeInterval[0] + carWidth / 2;
            var high = passageInterval.SafeInterval[1] - carWidth / 2;
            result.Add(low);
            result.Add(high);

            foreach (var x in distinctXs)
            {
                if (low < x && x < high)
                {
                    result.Add(x);
                }

                result.AddRange(differentWheelsWidths.Select(w => x + w).Where(x => low < x && x < high));
                result.AddRange(differentWheelsWidths.Select(w => x - w).Where(x => low < x && x < high));
            }

            return result.Order().Distinct().ToArray();
        }
    }
}
