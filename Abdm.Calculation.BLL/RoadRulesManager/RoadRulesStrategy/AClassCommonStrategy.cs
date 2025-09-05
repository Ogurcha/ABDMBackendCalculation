using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
{
    internal class AClassCommonStrategy : BaseRRStrategy
    {
        internal override RoadRules GetRoadRules()
        {
            var value = RoadRulesExtrensions.RR1;
            var valueSecondary = RoadRulesExtrensions.RR2;
            return Merge(value, valueSecondary);
        }
    }
}