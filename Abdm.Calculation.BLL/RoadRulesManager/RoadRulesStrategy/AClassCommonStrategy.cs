using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
{
    public class AClassCommonStrategy : BaseRRStrategy
    {
        public override List<LadingEnum> LadingIds => new List<LadingEnum> {
            LadingEnum.A8,
            LadingEnum.A11,
            LadingEnum.A14,
            LadingEnum.N_10,
            LadingEnum.N_13,
            LadingEnum.N_18,
            LadingEnum.N_30,
        };

        public override RoadRules GetRoadRules()
        {
            var value = RoadRulesExtensions.RR1;
            var valueSecondary = RoadRulesExtensions.RR2;
            return Merge(value, valueSecondary);
        }
    }
}