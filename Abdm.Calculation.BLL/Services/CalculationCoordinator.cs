using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

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
            IEnumerable<RoadRule> rules, 
            Mesh mesh)
        {
            var strainResults = new List<StrainResult>();
            foreach (var interval in intervals) {
                var trajectories = trajectorySelector.GetTrajectoriesStrainsMap(interval, rules, data);
                strainResults.AddRange(strainCalculator.GetStrainResultFromTrajectories(trajectories, interval, rules, data, mesh));
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
