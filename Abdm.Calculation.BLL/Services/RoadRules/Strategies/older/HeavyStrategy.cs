using System.ComponentModel;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies.older
{
    /// <summary>
    /// Нормы 1943–1962 — одиночные НК/НГ (колесные и гусеничные):
    /// вне совместного учёта с авто/пешеходами, не ближе 0,25 м к краю проезда
    /// от края обода/гусеницы.
    /// </summary>
    public class HeavyStrategy : BaseRRStrategy
    {
        public HeavyStrategy()
        {
            WorksInSnips =
            [
                SnipEnum.sn43,
                SnipEnum.sn48,
                SnipEnum.sn53,
                SnipEnum.sn62
            ];
        }

        public override List<LoadGroupTypeEnum> LoadGroupTypes =>
        [
            LoadGroupTypeEnum.Single,
            LoadGroupTypeEnum.Track,
            LoadGroupTypeEnum.NClass
        ];

        public override RoadRule[] GetRoadRules(LoadEnum loadId) => [RRHeavy];

        [Description("I. Одиночная НК/НГ, отступ 0,25 м от края проезда")]
        public static RoadRule RRHeavy => new RoadRule()
        {
            IsPedestrianAllowed = false,
            IsDynamicMovement = true,
            HasSafetyLine = true,
            MaxTrajectoriesInInterval = 1,
            MaxTrajectoriesTotal = 1,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalculation = false,
            ExtraBarrierDistance = 0.25d,
        };
    }
}
