using System.ComponentModel;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies.older
{
    /// <summary>
    /// Нормы 1938–1962 — автомобильные колонны (Н-10/Н-13/Н-18/Н-30):
    /// любое число колонн в пределах проезда, длина колонны не ограничена,
    /// мин. расстояние между осями колонн 2,8 м.
    /// </summary>
    public class ColumnStrategy : BaseRRStrategy
    {
        public ColumnStrategy()
        {
            WorksInSnips =
            [
                SnipEnum.sn38,
                SnipEnum.sn43,
                SnipEnum.sn48,
                SnipEnum.sn53,
                SnipEnum.sn62
            ];
        }

        public override List<LoadGroupTypeEnum> LoadGroupTypes =>
        [
            LoadGroupTypeEnum.AClass,
            LoadGroupTypeEnum.Common
        ];

        public override RoadRule[] GetRoadRules(LoadEnum loadId) => [RRColumn];

        [Description("I. Автомобильная колонна в пределах ширины проезда")]
        public static RoadRule RRColumn => new RoadRule()
        {
            IsDynamicMovement = true,
            MaxVehicleInTrajectory = int.MaxValue,
            MinTrajectoryDistance = 2.8,
            DoTrafficJamLoadCalculation = false,

            IsPedestrianAllowed = true,
            HasSafetyLine = true,
            MaxTrajectoriesInInterval = int.MaxValue,
            MaxTrajectoriesTotal = int.MaxValue,
            ExtraBarrierDistance = 0d,
        };
    }
}
