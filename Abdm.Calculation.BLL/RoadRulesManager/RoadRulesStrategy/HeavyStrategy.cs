using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
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
            return RoadRulesExtensions.RR3;
        }
    }
}