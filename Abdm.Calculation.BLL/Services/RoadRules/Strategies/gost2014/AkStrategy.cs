using System.ComponentModel;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies.gost2014
{
    /// <summary>
    /// ГОСТ 32960-2014, п. 4.4.1–4.4.2 — вариант 1: нагрузка АК с пешеходами без ограничений.
    /// </summary>
    public class AkStrategy : BaseRRStrategy
    {
        public AkStrategy()
        {
            WorksInSnips = [SnipEnum.gost2014];
        }

        public override List<LoadGroupTypeEnum> LoadGroupTypes =>
        [
            LoadGroupTypeEnum.AClass
        ];

        public override RoadRule[] GetRoadRules(LoadEnum loadId) => [RRAk];

        [Description("I. \"АК\" с пешеходами (от ограждения, габарит Г)")]
        public static RoadRule RRAk => new RoadRule()
        {
            IsDynamicMovement = true,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalculation = true,

            IsPedestrianAllowed = true,
            HasSafetyLine = false,
            MaxTrajectoriesInInterval = int.MaxValue,
            MaxTrajectoriesTotal = int.MaxValue,
        };
    }
}
