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
            var unpopulated = new List<StrainResultUnpopulated>();
            foreach (var interval in intervals) {
                var strainsMap = strainCalculator.GenerateStrainsMap(interval, data);
                unpopulated.AddRange(strainSelector.SelectBestStrainResult(strainsMap, interval, data));
            }

            return strainResultPopulator.PopulateStrainResults(unpopulated, data.Data).ToArray();
        }
    }
}
