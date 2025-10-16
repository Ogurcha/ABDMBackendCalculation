using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class CalculationCoordinator(
        ITrajectorySelector trajectorySelector,
        IStrainCalculator strainCalculator,
        IPassTypeResolver passTypeResolver) : ICalculationCoordinator
    {
        public PassTypeEnum GetPassType(
            PassTypeSmallModel data,
            IEnumerable<IntervalModel> intervals,
            IEnumerable<RoadRule> rules)
        {
            var strainResults = new List<StrainResult>();
            foreach (var interval in intervals) {
                var trajectories = trajectorySelector.GetTrajectoriesStrainsMap(interval, rules, data);
                strainResults.AddRange(strainCalculator.Calculate(trajectories, interval, rules, data));
            }
            strainResults = strainResults.GroupBy(x => x.RoadRuleRef).Select(x => new StrainResult() { 
                RoadRuleRef = x.Key, 
                Strain = x.Select(s => s.Strain).Sum(), 
                StrainOneAuto = x.Select(s => s.StrainOneAuto).Max()
            }).ToList();

            return passTypeResolver.Resolve(strainResults, data.Surface);
        }
    }
}
