using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCalculator : IStrainCalculator
    {
        public IEnumerable<StrainResult> Calculate(
        Dictionary<RoadRule, (double X, double Strain)[]> trajectoriesMap,
        IntervalModel intervalModel,
        IEnumerable<RoadRule> roadRules,
        PassTypeSmallModel data)
        {
            foreach (var roadRule in roadRules)
            {
                var actualVehicleCount = Math.Min(roadRule.MaxVehicleCount, intervalModel.PassageIntervalRef.LaneCount);
                yield return
                    new StrainResult
                    {
                        RoadRuleRef = roadRule,
                        Strain = trajectoriesMap[roadRule].Take(actualVehicleCount).Count() == actualVehicleCount
                        ? trajectoriesMap[roadRule].Take(actualVehicleCount).Sum(x => x.Strain)
                        : trajectoriesMap[roadRule].First().Strain * actualVehicleCount,
                        StrainOneAuto = trajectoriesMap[roadRule].First().Strain
                    };
            }
        }
    }
}
