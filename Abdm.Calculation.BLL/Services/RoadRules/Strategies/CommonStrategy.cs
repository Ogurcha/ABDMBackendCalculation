using System.ComponentModel;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies
{
    public class CommonStrategy : BaseRRStrategy
    {
        public override List<LoadGroupTypeEnum> LoadGroupTypes => new List<LoadGroupTypeEnum> {
            LoadGroupTypeEnum.Common
        };

        public override RoadRule[] GetRoadRules(LoadEnum loadId)
        {
            if (loadId == LoadEnum.EN3)
            {
                return [RRCommon2Vehicles, RRCommon2VehiclesNoSafetyLine];
            }
            return [RRCommon, RRCommonNoSafetyLine];
        }

        [Description("I. \"Общего назначения\" без заезда на полосу безопасности")]
        public static RoadRule RRCommon => new RoadRule()
        {
            IsDynamicMovement = true,
            MaxVehicleInTrajectory = int.MaxValue,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalculation = false,

            IsPedestrianAllowed = false,
            HasSafetyLine = true,
            MaxTrajectoriesInInterval = 2,
            MaxTrajectoriesTotal = int.MaxValue,
        };

        [Description("II. \"Общего назначения\" с заездом на полосу безопасности")]
        public static RoadRule RRCommonNoSafetyLine => new RoadRule()
        {
            IsDynamicMovement = true,
            MaxVehicleInTrajectory = int.MaxValue,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalculation = false,

            IsPedestrianAllowed = true,
            HasSafetyLine = false,
            MaxTrajectoriesInInterval = int.MaxValue,
            MaxTrajectoriesTotal = int.MaxValue,
        };

        [Description("I(1). Вариант – не более 2-х грузовиков в колонне")]
        public static RoadRule RRCommon2Vehicles { 
            get 
            {
                var rr = RRCommon;
                rr.MaxVehicleInTrajectory = 2;
                return rr;
            } 
        }

        [Description("II(1). Вариант – не более 2-х грузовиков в колонне")]
        public static RoadRule RRCommon2VehiclesNoSafetyLine
        {
            get
            {
                var rr = RRCommonNoSafetyLine;
                rr.MaxVehicleInTrajectory = 2;
                return rr;
            }
        }
    }
}