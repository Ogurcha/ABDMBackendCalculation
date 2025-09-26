using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IRoadRulesFactory
    {
        public RoadRules? CreateRoadRuleStrategy(LoadEnum loadId);
    }
}