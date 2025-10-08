using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.Algorithmic;
using Abdm.Calculation.BLL.Models.Parameters;
using Abdm.Calculation.DAL;
using Mapster;

namespace Abdm.Calculation.BLL.Services
{
    public class PassageIntervalService(IPassageIntervalRepository passageIntervalRepository) : IPassageIntervalService
    {
        /// <summary>
        /// Возвращает абсолютные значения  интервалов для данного иссо
        /// </summary>
        public async Task<PassageInterval[]> GetPassageIntervals(long issoId, 
            double globalPositionShift, 
            CancellationToken cancellationToken)
        {
            var queryResult = await passageIntervalRepository.GetPassageIntervals(issoId, cancellationToken);
            var passageIntervals = queryResult.Adapt<PassageInterval[]>();

            var filteredIntervals = FilterIntervals(passageIntervals);

            double rightSideExtraShift = default;
            var right = filteredIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.RightInterval).FirstOrDefault();
            if (right != null)
            {
                var fenceSize = right.AbsolutePositionLeft;
                var leftSideSize = filteredIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.LeftInterval)
                    .First().TotalWidth;
                rightSideExtraShift = fenceSize + leftSideSize;
            }

            foreach (var intervalModel in filteredIntervals)
            {
                if (intervalModel.Type != Enums.PassageIntervalTypeEnum.RightInterval)
                {
                    intervalModel.AbsolutePositionLeft = globalPositionShift;
                    intervalModel.AbsolutePositionRight = globalPositionShift + intervalModel.TotalWidth;
                }
                else
                {
                    intervalModel.AbsolutePositionLeft = globalPositionShift + rightSideExtraShift;
                    intervalModel.AbsolutePositionLeft = globalPositionShift + rightSideExtraShift + intervalModel.TotalWidth;
                }
            }

            return filteredIntervals;
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
            PassageInterval passageInterval,
            LoadSchema loadSchema,
            RoadRule[] roadRules)
        {
            var safeCarWidth = roadRules.Max(x => x.MinColumnDistance);
            if (loadSchema.Width != null)
            {
                safeCarWidth = Math.Max(loadSchema.Width.Value, safeCarWidth);
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

        /// <summary>
        /// Фильтрация, чтобы избавиться от дублей:
        /// в бд странно хранятся интервалы. 
        /// Один и тот же промежуток может быть записан два раза
        /// (Как одинарный интервал и как сумма двух интервалов). 
        /// Есть догадка, что дубли связаны с тем, 
        /// что если интервал содержит две полосы 
        /// и на нем нет ограждений, то этот интервал 
        /// можно использовать как двуполосное движение 
        /// для маеленьких машин, так и однополосное для больших
        /// </summary>
        private PassageInterval[] FilterIntervals(PassageInterval[]? passageIntervals)
        {
            if (passageIntervals?.Any() != true)
            {
                return [];
            }
            var left = passageIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.LeftInterval);
            var right = passageIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.RightInterval);
            var whole = passageIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.WholeInterval);
            if (
                left.Count() > 0 && 
                left.Count() == right.Count()
                )
            {
                return left.Concat(right).ToArray();
            }
            return whole.ToArray();
        }
    }
}
