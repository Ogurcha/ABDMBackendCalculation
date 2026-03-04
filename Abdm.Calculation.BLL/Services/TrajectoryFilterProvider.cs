using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Helpers;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Генератор фильтров, которые в свою очередь проверяют валидность траектории с учетом характеристик интервала, Правил движения и характеристик нагрузки
    /// </summary>
    /// <param name="equalityComparer"></param>
    public class TrajectoryFilterProvider(IEqualityComparer<double> equalityComparer) : ITrajectoryFilterProvider
    {
        /// <summary>
        /// возвращает фильтры, которые в свою очередь проверяют валидность траектории с учетом характеристик интервала, Правил движения и характеристик нагрузки. 
        /// Фильтров вернётся меньше, чем кол-во <paramref name="roadRules"/>, так как <paramref name="roadRules"/> с одинаковыми характеристиками будут использовать один и тот же фильтр
        /// </summary>
        public VehicleTrajectoryFilter[] GetFilters(PassageInterval passageInterval, 
            LoadModel load, 
            IEnumerable<RoadRule> roadRules)
        {
            var groupedBySafetyLine = roadRules.GroupBy(r => (
            actualSafetyLineLeft: r.HasSafetyLine ? passageInterval.SafetyLineLeft : (double)default,
            actualSafetyLineRight: r.HasSafetyLine ? passageInterval.SafetyLineRight : (double)default));

            var distance = PassTypeFormulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(load, roadRules);

            var result = new List<VehicleTrajectoryFilter>();

            foreach (var grouped in groupedBySafetyLine)
            {
                var filter = GetFilter(passageInterval, 
                    grouped.Key.actualSafetyLineLeft, 
                    grouped.Key.actualSafetyLineRight, 
                    distance);

                result.AddRange(grouped.Select(r => new VehicleTrajectoryFilter() { 
                     Filter = filter.func,
                     RoadRuleRef = r,
                     EdgeCaseLeft = filter.edgeCaseLeft,
                     EdgeCaseRight = filter.edgeCaseRight
                }));
            }

            return result.ToArray();
        }

        /// <summary>
        /// возвращает фильтры, которые в свою очередь проверяют валидность траектории с учетом характеристик интервала, Правил движения и характеристик нагрузки
        /// </summary>
        public VehicleTrajectoryFilter GetFilter(PassageInterval passageInterval, 
            LoadModel load, 
            RoadRule roadRule)
        {
            var actualSafetyLineLeft = roadRule.HasSafetyLine ? passageInterval.SafetyLineLeft : (double)default;
            var actualSafetyLineRight = roadRule.HasSafetyLine ? passageInterval.SafetyLineRight : (double)default;
            var distance = PassTypeFormulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(load, [roadRule]);
            var result = GetFilter(passageInterval, actualSafetyLineLeft, actualSafetyLineRight, distance);
            return new VehicleTrajectoryFilter()
            {
                Filter = result.func,
                RoadRuleRef = roadRule,
                EdgeCaseLeft = result.edgeCaseLeft,
                EdgeCaseRight = result.edgeCaseRight
            };
        }

        private (Func<double, bool> func, double edgeCaseLeft, double edgeCaseRight) GetFilter(PassageInterval passageInterval, 
            double actualSafetyLineLeft, 
            double actualSafetyLineRight, 
            double distanceBetweenIntervalEdgeAndTrajectoryCenter)
        {
            var start = passageInterval.AbsolutePositionLeft
                + actualSafetyLineLeft
                + distanceBetweenIntervalEdgeAndTrajectoryCenter;

            var finish = passageInterval.AbsolutePositionRight
                - actualSafetyLineRight
                - distanceBetweenIntervalEdgeAndTrajectoryCenter;

            return (new Func<double, bool>(x =>
                (x >= start || equalityComparer.Equals(x, start))
                && (x <= finish || equalityComparer.Equals(x, finish)))
                , start
                , finish);
        }
    }
}
