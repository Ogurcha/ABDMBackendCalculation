using System.ComponentModel;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies.older
{
    /// <summary>
    /// Гушосдор 1938 — гусеничные/одиночные нагрузки в любое положение по ширине проезда
    /// (без дополнительного отступа 0,25 м).
    /// </summary>
    public class Sn38HeavyStrategy : BaseRRStrategy
    {
        public Sn38HeavyStrategy()
        {
            WorksInSnips = [SnipEnum.sn38];
        }

        public override List<LoadGroupTypeEnum> LoadGroupTypes =>
        [
            LoadGroupTypeEnum.Single,
            LoadGroupTypeEnum.Track,
            LoadGroupTypeEnum.NClass
        ];

        public override RoadRule[] GetRoadRules(LoadEnum loadId) => [RRHeavy];

        [Description("I. Одиночная гусеничная/колёсная нагрузка без доп. отступа от края")]
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
            ExtraBarrierDistance = 0d,
        };
    }
}
