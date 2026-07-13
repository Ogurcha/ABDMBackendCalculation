using System.ComponentModel;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class VehicleColumnStrategy : BaseRRStrategy
    {
        public override List<LoadGroupTypeEnum> LoadGroupTypes => new List<LoadGroupTypeEnum> {
            LoadGroupTypeEnum.AClass
        };

        public override RoadRule[] GetRoadRules(LoadEnum loadId)
        {
            return [RRVehicleColumn, RRVehicleColumnNoSafetyLine];
        }

        [Description("I. \"АК\" без заезда на полосу безопасности")]
        public static RoadRule RRVehicleColumn => new RoadRule()
        {
            IsDynamicMovement = true,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalculation = true,

            IsPedestrianAllowed = true,
            HasSafetyLine = false,
            MaxTrajectoriesInInterval = 2,
            MaxTrajectoriesTotal = int.MaxValue,
        };

        [Description("II. \"АК\" с заездом на полосу безопасности")]
        public static RoadRule RRVehicleColumnNoSafetyLine => new RoadRule()
        {
            IsDynamicMovement = true,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalculation = true,

            IsPedestrianAllowed = false,
            HasSafetyLine = true,
            MaxTrajectoriesInInterval = 2,
            MaxTrajectoriesTotal = 2,
        };
    }
}
