using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
{
    public class EN3Strategy : BaseRRStrategy
    {
        public override List<LadingEnum> LadingIds => new List<LadingEnum> { 
            LadingEnum.EN3 
        };

        public override RoadRules GetRoadRules()
        {
            var value = RoadRulesExtensions.RR1_1;
            var valueSecondary = RoadRulesExtensions.RR2_1;
            return Merge(value, valueSecondary);
        }
    }
}
