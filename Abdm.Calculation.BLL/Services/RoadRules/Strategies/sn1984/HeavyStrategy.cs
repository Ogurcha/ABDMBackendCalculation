using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies.sn1984
{
    /// <summary>
    /// СНиП 2.05.03-84 / СП 35 — одиночные НК/НГ вне полос безопасности, без пешеходов.
    /// </summary>
    public class HeavyStrategy : BaseRRStrategy
    {
        public HeavyStrategy()
        {
            WorksInSnips = [SnipEnum.sn84, SnipEnum.sp35];
        }

        public override List<LoadGroupTypeEnum> LoadGroupTypes =>
        [
            LoadGroupTypeEnum.Single,
            LoadGroupTypeEnum.Track,
            LoadGroupTypeEnum.NClass
        ];

        public override RoadRule[] GetRoadRules(LoadEnum loadId) => [RoadRulesConstants.RR3];
    }
}
