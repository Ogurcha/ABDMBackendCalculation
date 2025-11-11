using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class HeavyStrategy : BaseRRStrategy
    {
        public override List<LoadGroupTypeEnum> LoadGroupTypes => new List<LoadGroupTypeEnum>
        {
            LoadGroupTypeEnum.Single,
            LoadGroupTypeEnum.Track,
            LoadGroupTypeEnum.NClass
        };

        public override RoadRule[] GetRoadRules(LoadEnum LoadId)
        {
            return [RoadRulesConstants.RR3];
        }
    }
}