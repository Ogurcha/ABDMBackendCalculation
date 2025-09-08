using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
{
    public class AbStrategy : BaseRRStrategy
    {
        public override List<LadingEnum> LadingIds => new List<LadingEnum> {
            LadingEnum.AB51,
            LadingEnum.AB74,
            LadingEnum.AB151,
        };

        public override RoadRules GetRoadRules()
        {
            return RoadRulesExtensions.RR4;
        }
    }
}