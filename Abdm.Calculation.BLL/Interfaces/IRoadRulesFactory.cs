using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IRoadRulesFactory
    {
        public RoadRule[]? CreateRoadRuleStrategy(LoadEnum loadId);
    }
}