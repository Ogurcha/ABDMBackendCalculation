using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class AbStrategy : BaseRRStrategy
    {
        public override List<LoadGroupTypeEnum> LoadGroupTypes => new List<LoadGroupTypeEnum> {
            LoadGroupTypeEnum.AB,
        };

        public override RoadRule[] GetRoadRules(LoadEnum load)
        {
            return [RoadRulesConstants.RR5];
        }
    }
}