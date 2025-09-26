using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
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

        public override RoadRules GetRoadRules()
        {
            var value = RoadRulesExtensions.RR1;
            var valueSecondary = RoadRulesExtensions.RR2;
            return Merge(value, valueSecondary);
        }
    }
}