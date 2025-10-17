using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class EN3Strategy : BaseRRStrategy
    {
        public override List<LoadEnum> LoadIds => new List<LoadEnum> { 
            LoadEnum.EN3,
            LoadEnum.N_10,
            LoadEnum.N_13,
            LoadEnum.N_18,
            LoadEnum.N_30,
        };

        public override RoadRule[] GetRoadRules()
        {
            var value = RoadRulesConstants.RR1_1;
            var valueSecondary = RoadRulesConstants.RR2_1;
            return [value, valueSecondary];
        }
    }
}
