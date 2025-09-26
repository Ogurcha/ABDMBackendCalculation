using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy;

namespace Abdm.Calculation.BLL.RoadRulesManager
{
    /// <summary>
    /// фэктори, возвращающий правила движения по ИССО.
    /// </summary>
    public class RoadRulesFactory 
        (List<BaseRRStrategy> strategies)
        : IRoadRulesFactory 
    {
        public RoadRules? CreateRoadRuleStrategy(LoadEnum loadId) 
            => strategies.FirstOrDefault(s => s.LoadIds.Contains(loadId))?.GetRoadRules();
    }
}
