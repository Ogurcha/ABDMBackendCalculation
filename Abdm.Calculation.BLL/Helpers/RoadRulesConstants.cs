using System.ComponentModel;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Helpers
{
    /// <summary>
    /// правила проверки по нормам для каждого типа нагрузок
    /// </summary>
    public static class RoadRulesConstants
    {
        [Description("I. \"Общего назначения\" и \"АК\" без заезда на полосу безопасности")]
        public static RoadRule RR1 => new RoadRule()
        {
            IsPedestrianAllowed = true,
            IsDynamicMovement = true,
            HasSafetyLine = false,
            MaxVehicleCount = int.MaxValue,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalulation = true,
        };

        [Description("I(1). Вариант – не более 2-х грузовиков в колонне")]
        public static RoadRule RR1_1 => new RoadRule()
        {
            IsPedestrianAllowed = true,
            IsDynamicMovement = true,
            HasSafetyLine = false,
            MaxVehicleCount = int.MaxValue,
            MaxVehicleInTrajectory = 2,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalulation = false,
        };

        [Description("II. \"Общего назначения\" и \"АК\" с заездом на полосу безопасности")]
        public static RoadRule RR2 => new RoadRule()
        {
            IsPedestrianAllowed = false,
            IsDynamicMovement = true,
            HasSafetyLine = true,
            MaxVehicleCount = 2,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalulation = true,
        };

        [Description("II(1). Вариант – не более 2-х грузовиков в колонне")]
        public static RoadRule RR2_1 => new RoadRule()
        {
            IsPedestrianAllowed = false,
            IsDynamicMovement = true,
            HasSafetyLine = true,
            MaxVehicleCount = 2,
            MaxVehicleInTrajectory = 2,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalulation = false,
        };

        [Description("III. \"Одиночная\" без заезда на полосу безоп.")]
        public static RoadRule RR3 => new RoadRule()
        {
            IsPedestrianAllowed = false,
            IsDynamicMovement = true,
            HasSafetyLine = false,
            MaxVehicleCount = 1,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalulation = false,
        };

        [Description("IV. \"Специальная АБ\" движущаяся")]
        public static RoadRule RR4 => new RoadRule()
        {
            IsPedestrianAllowed = true,
            IsDynamicMovement = true,
            HasSafetyLine = true,
            MaxVehicleCount = 1,
            MaxVehicleInTrajectory = 1,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalulation = false,
        };
    }
}
