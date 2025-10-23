using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class AClassCommonStrategy : BaseRRStrategy
    {
        public override List<LoadGroupTypeEnum> LoadGroupTypes => new List<LoadGroupTypeEnum> {
            LoadGroupTypeEnum.Common,
            LoadGroupTypeEnum.AClass
        };

        public override RoadRule[] GetRoadRules(LoadEnum loadId)
        {
            if (loadId == LoadEnum.EN3)
            {
                return [RoadRulesConstants.RR1_1, RoadRulesConstants.RR2_1];
            }
            return [RoadRulesConstants.RR1, RoadRulesConstants.RR2];
        }
    }
}