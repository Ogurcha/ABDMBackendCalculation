using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainResultService(
        IStrainCalculator strainCalculator,
        IStrainSelector strainSelector,
        IStrainResultPopulator strainResultPopulator) : IStrainResultService
    {
        public StrainResult[] GetStrainResults(
            VehicleRollingBigModel data,
            IEnumerable<IntervalModel> intervals)
        {
            var strainResults = new List<StrainResultUnpopulated>();
            foreach (var interval in intervals) {
                var strainsMap = strainCalculator.GetStrainsMap(interval, data);
                strainResults.AddRange(strainSelector.GetStrainResults(strainsMap, interval, data));
            }            

            return strainResults.Select(x => strainResultPopulator.PopulateStrainResult(x, data.Data)).ToArray();
        }
    }
}
