using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
{
    internal class AbStrategy : BaseRRStrategy
    {
        internal override RoadRules GetRoadRules()
        {
            return RoadRulesExtrensions.RR4;
        }
    }
}