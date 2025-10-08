using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class HeavyStrategy : BaseRRStrategy
    {
        public override List<LoadEnum> LoadIds => new List<LoadEnum> {
            LoadEnum.NG60,
            LoadEnum.NG30,
            LoadEnum.T60,
            LoadEnum.T25,
            LoadEnum.N11,
            LoadEnum.N14
        };

        public override RoadRules GetRoadRules()
        {
            return RoadRulesConstants.RR3;
        }
    }
}