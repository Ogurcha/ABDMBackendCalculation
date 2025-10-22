using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainResultService(
        IStrainCalculator strainCalculator,
        IStrainSelector strainSelector) : IStrainResultService
    {
        public List<StrainResult> GetStrainResults(
            PassTypeSmallModel data,
            IEnumerable<IntervalModel> intervals,
            IEnumerable<RoadRule> rules, 
            Mesh mesh)
        {
            var strainResults = new List<StrainResult>();
            foreach (var interval in intervals) {
                var strainsMap = strainCalculator.GetStrainsMap(interval, rules, data);
                strainResults.AddRange(strainSelector.GetStrainResults(strainsMap, interval, rules, data, mesh));
            }
            strainResults = strainResults.GroupBy(x => x.RoadRuleRef).Select(x => new StrainResult() { 
                RoadRuleRef = x.Key, 
                Strain = x.Select(s => s.Strain).Sum(), 
                StrainOneAuto = x.Select(s => s.StrainOneAuto).Max()
            }).ToList();

            return strainResults;
        }
    }
}
