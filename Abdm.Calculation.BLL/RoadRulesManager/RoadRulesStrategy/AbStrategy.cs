using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy
{
    public class AbStrategy : BaseRRStrategy
    {
        public override List<LoadEnum> LoadIds => new List<LoadEnum> {
            LoadEnum.AB51,
            LoadEnum.AB74,
            LoadEnum.AB151,
        };

        public override RoadRules GetRoadRules()
        {
            return RoadRulesExtensions.RR4;
        }
    }
}