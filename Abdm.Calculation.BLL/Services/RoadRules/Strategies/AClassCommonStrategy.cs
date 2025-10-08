using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class AClassCommonStrategy : BaseRRStrategy
    {
        public override List<LoadEnum> LoadIds => new List<LoadEnum> {
            LoadEnum.A8,
            LoadEnum.A11,
            LoadEnum.A14,
            LoadEnum.N_10,
            LoadEnum.N_13,
            LoadEnum.N_18,
            LoadEnum.N_30,
        };

        public override RoadRules[] GetRoadRules()
        {
            var value = RoadRulesConstants.RR1;
            var valueSecondary = RoadRulesConstants.RR2;
            return [value, valueSecondary];
        }
    }
}