using System.ComponentModel;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.RoadRules.Strategies.gost2014
{
    /// <summary>
    /// ГОСТ 32960-2014, п. 4.4.1 / 4.4.4 — вариант 2: нагрузка НК вне полос безопасности.
    /// MaxVehicleInTrajectory = 2 позволяет сравнить одиночную и сдвоенную НК
    /// с учётом GetMultiVehicleCoefficient (0,75).
    /// </summary>
    public class NkStrategy : BaseRRStrategy
    {
        public NkStrategy()
        {
            WorksInSnips = [SnipEnum.gost2014];
        }

        public override List<LoadGroupTypeEnum> LoadGroupTypes =>
        [
            LoadGroupTypeEnum.NClass,
            LoadGroupTypeEnum.Single
        ];

        public override RoadRule[] GetRoadRules(LoadEnum loadId) => [RRNk];

        [Description("I. \"НК\" вне полос безопасности (одиночная / сдвоенная через multi-vehicle coeff)")]
        public static RoadRule RRNk => new RoadRule()
        {
            IsPedestrianAllowed = false,
            IsDynamicMovement = true,
            HasSafetyLine = true,
            MaxTrajectoriesInInterval = 1,
            MaxTrajectoriesTotal = 1,
            MaxVehicleInTrajectory = 2,
            MinTrajectoryDistance = 3,
            DoTrafficJamLoadCalculation = false,
        };
    }
}
