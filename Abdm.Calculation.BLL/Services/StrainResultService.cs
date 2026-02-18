using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainResultService(
        IStrainCalculator strainCalculator,
        IStrainSelector strainSelector) : IStrainResultService
    {
        public List<StrainResult> GetStrainResults(
            VehicleRollingBigModel data,
            IEnumerable<IntervalModel> intervals)
        {
            var strainResults = new List<StrainResult>();
            foreach (var interval in intervals) {
                var strainsMap = strainCalculator.GetStrainsMap(interval, data);
                strainResults.AddRange(strainSelector.GetStrainResults(strainsMap, interval, data));
            }

            return strainResults;
        }
    }
}
