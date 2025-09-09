using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
{
    public class HeavyStrategy : BaseRRStrategy
    {
        public override List<LadingEnum> LadingIds => new List<LadingEnum> {
            LadingEnum.NG60,
            LadingEnum.NG30,
            LadingEnum.T60,
            LadingEnum.T25,
            LadingEnum.N11,
            LadingEnum.N14
        };

        public override RoadRules GetRoadRules()
        {
            return RoadRulesExtensions.RR3;
        }
    }
}