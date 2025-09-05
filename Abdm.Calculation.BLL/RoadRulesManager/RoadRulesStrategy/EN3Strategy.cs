using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
{
    internal class EN3Strategy : BaseRRStrategy
    {
        internal override RoadRules GetRoadRules()
        {
            var value = RoadRulesExtrensions.RR1_1;
            var valueSecondary = RoadRulesExtrensions.RR2_1;
            return Merge(value, valueSecondary);
        }
    }
}
