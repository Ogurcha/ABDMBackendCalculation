using System.ComponentModel;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Helpers
{
    /// <summary>
    /// правила проверки по нормам для каждого типа нагрузок
    /// </summary>
    public static class RoadRulesConstants
    {
        [Description("III. \"Одиночная\" без заезда на полосу безоп.")]
        public static RoadRule RR3 => new RoadRule()
        {
            IsPedestrianAllowed = false,
            IsDynamicMovement = true,
            HasSafetyLine = false,
            MaxTrajectoriesCount = 1,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalulation = false,
        };

        [Description("V. \"Специальная АБ\" не движущаяся")]
        public static RoadRule RR5 => new RoadRule()
        {
            IsPedestrianAllowed = true,
            IsDynamicMovement = true,
            HasSafetyLine = true,
            MaxTrajectoriesCount = int.MaxValue,
            MaxVehicleInTrajectory = 3,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalulation = false,
        };
    }
}
