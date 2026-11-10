using System.ComponentModel;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies.sn1984
{
    /// <summary>
    /// СНиП 2.05.03-84 / СП 35.13330.2011 — нагрузка АК: два случая размещения (с/без полос безопасности).
    /// </summary>
    public class AkStrategy : BaseRRStrategy
    {
        public AkStrategy()
        {
            WorksInSnips = [SnipEnum.sn84, SnipEnum.sp35];
        }

        public override List<LoadGroupTypeEnum> LoadGroupTypes =>
        [
            LoadGroupTypeEnum.AClass,
            LoadGroupTypeEnum.Common
        ];

        public override RoadRule[] GetRoadRules(LoadEnum loadId) =>
            [RRAk, RRAkNoSafetyLine];

        [Description("I. \"АК\" без заезда на полосу безопасности")]
        public static RoadRule RRAk => new RoadRule()
        {
            IsDynamicMovement = true,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalculation = true,

            IsPedestrianAllowed = false,
            HasSafetyLine = true,
            MaxTrajectoriesInInterval = 2,
            MaxTrajectoriesTotal = int.MaxValue,
        };

        [Description("II. \"АК\" с заездом на полосу безопасности")]
        public static RoadRule RRAkNoSafetyLine => new RoadRule()
        {
            IsDynamicMovement = true,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalculation = true,

            IsPedestrianAllowed = true,
            HasSafetyLine = false,
            MaxTrajectoriesInInterval = 2,
            MaxTrajectoriesTotal = 2,
        };
    }
}
