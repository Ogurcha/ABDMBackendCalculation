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
        public RoadRule[]? CreateRoadRuleStrategy(LoadGroupTypeEnum type, LoadEnum loadId, SnipEnum snip)
        {
            var strategy = strategies.FirstOrDefault(s => s.LoadGroupTypes.Contains(type) && s.WorksInSnips.Contains(snip))
                ?? strategies.FirstOrDefault(s => s.LoadGroupTypes.Contains(type) && s.WorksInSnips.Contains(SnipEnum.odm16));

            return strategy?.GetRoadRules(loadId);
        }
    }
}
