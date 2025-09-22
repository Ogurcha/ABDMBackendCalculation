using Abdm.Calculation.BLL.Entities;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL;
using Mapster;

namespace Abdm.Calculation.BLL.Services
{
    public class PassageIntervalService(IPassageIntervalRepository passageIntervalRepository) : IPassageIntervalService
    {
        private const double slExtraDistance = 0.25;

        /// <summary>
        /// Возвращает данные для расщета интервалов для данного иссо
        /// </summary>
        public async Task<PassageIntervalModel[]> GetPassageIntervals(long issoId, CancellationToken cancellationToken)
        {
            var queryResult = await passageIntervalRepository.GetPassageIntervals(issoId, cancellationToken);
            var passageIntervals = queryResult.Adapt<PassageIntervalModel[]>();

            foreach (var passageInterval in passageIntervals)
            {
                passageInterval.SafeInterval = [
                    passageInterval.SafetyLineLeft > slExtraDistance ? passageInterval.SafetyLineLeft : slExtraDistance + passageInterval.SafetyLineLeft,
                    passageInterval.SafetyLineRight > slExtraDistance ? passageInterval.TotalWidth - passageInterval.SafetyLineRight : passageInterval.TotalWidth - passageInterval.SafetyLineRight - slExtraDistance
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
        public double[] CalculateDistinctXPositionsIncludingWheelOffsets(
            double[] distinctXs,
            PassageIntervalModel passageInterval,
            AxleModel[] axles,
            double carWidth)
        {
            var result = new List<double>();

            var differentWheelsWidths = axles.SelectMany(axle => axle?.Wheels ?? [])
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
