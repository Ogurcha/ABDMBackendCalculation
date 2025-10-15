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
            var strainResults = intervals
                .SelectMany(interval =>
                {
                    var trajectories = trajectorySelector.GetTrajectoriesStrainsMap(interval, rules, data);
                    return strainCalculator.Calculate(trajectories, interval, rules, data);
                })
                .ToList();

            return passTypeResolver.Resolve(strainResults, data.Surface);
        }
    }
}
