using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class AbStrategy : BaseRRStrategy
    {
        public override List<LoadEnum> LoadIds => new List<LoadEnum> {
            LoadEnum.AB51,
            LoadEnum.AB74,
            LoadEnum.AB151,
        };

        public override RoadRule[] GetRoadRules()
        {
            return [RoadRulesConstants.RR4];
        }
    }
}