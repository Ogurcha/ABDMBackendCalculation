using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Services.RoadRules.Strategies;

namespace Abdm.Calculation.BLL.Services.RoadRules
{
    /// <summary>
    /// фэктори, возвращающий правила движения по ИССО.
    /// </summary>
    public class RoadRulesFactory 
        (List<BaseRRStrategy> strategies)
        : IRoadRulesFactory 
    {
        public RoadRule[]? CreateRoadRuleStrategy(LoadEnum loadId) 
            => strategies.FirstOrDefault(s => s.LoadIds.Contains(loadId))?.GetRoadRules();
    }
}
