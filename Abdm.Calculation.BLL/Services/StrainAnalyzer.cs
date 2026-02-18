using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainAnalyzer : IStrainAnalyzer
    {

        public Summary Analyze(
            VehicleRollingResult defaultRolling,
            VehicleRollingResult mirroredRolling,
            VehicleRollingBigModel dataModel)
        {
            var strains = defaultRolling.StrainResults.Union(mirroredRolling.StrainResults)
                .GroupBy(x => x.RoadRuleRef);
            return new Summary();
        }
    }
}
