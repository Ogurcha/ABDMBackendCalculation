using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class EN3Strategy : BaseRRStrategy
    {
        public override List<LoadEnum> LoadIds => new List<LoadEnum> { 
            LoadEnum.EN3 
        };

        public override RoadRules[] GetRoadRules()
        {
            var value = RoadRulesConstants.RR1_1;
            var valueSecondary = RoadRulesConstants.RR2_1;
            return [value, valueSecondary];
        }
    }
}
