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
                passageInterval.SafetyLineLeft = passageInterval.SafetyLineLeft > slExtraDistance ? passageInterval.SafetyLineLeft : slExtraDistance + passageInterval.SafetyLineLeft;
                passageInterval.SafetyLineRight = passageInterval.SafetyLineRight > slExtraDistance ? passageInterval.TotalWidth - passageInterval.SafetyLineRight : passageInterval.TotalWidth - passageInterval.SafetyLineRight - slExtraDistance;
            }

            return passageIntervals;
        }

        /// <summary>
        /// Добирает координаты для проверок с учётом размера тележек
        /// </summary>
        /// <param name="distinctXs">Массив точек по оси Х для всей поверхности ИССО</param>
        /// <param name="passageInterval">Интервал проезда по оси Х, по которому должно проехать ТС</param>
        /// <param name="axles">Информация о Тележках транспортного средства</param>
        /// <param name="carWidth">Общая ширина ТС</param>
        /// <returns>Массив точек по оси Х внутри данного интервала, и с учётом заездов и с учётом размера колёс</returns>
        public VehicleXPosition[] CalculateVehiclePositionsIncludingWheelOffsets(
            double[] distinctXs,
            PassageIntervalModel passageInterval,
            LoadSchema loadSchema,
            RoadRules roadRules)
        {
            var safeCarWidth = roadRules.MinColumnDistance;
            if (loadSchema.Width != null)
            {
                safeCarWidth = Math.Max(loadSchema.Width.Value, roadRules.MinColumnDistance);
            }

            var result = new List<VehicleXPosition>();
            var halfCarWidth = safeCarWidth / 2;
            var halfWheelOffsets = loadSchema.Axles.SelectMany(axle => axle?.WheelsDistance ?? [])
                .Distinct().Select(a => a / 2).ToArray();

            var low = passageInterval.SafetyLineLeft + halfCarWidth;
            var high = passageInterval.SafetyLineRight - halfCarWidth;
            result.Add(new VehicleXPosition(low, halfWheelOffsets));
            result.Add(new VehicleXPosition(high, halfWheelOffsets));

            foreach (var x in distinctXs)
            {
                if (low < x && x < high)
                {
                    result.Add(new VehicleXPosition(x, halfWheelOffsets));  
                }
            }

            return result.OrderBy(x => x.CenterXPosition).ToArray();
        }
    }
}
