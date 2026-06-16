namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// Итоговое напряжение, выдваемое колонной автомобилей
    /// на определённоой траектории
    /// Уникально для каждой пары <see cref="RoadRule"/> и <see cref="VehicleTrajectory"/>
    /// </summary>
    public class VehicleColumnStrain : ComparableStrainBase
    {
        /// <summary>
        /// Итоговое напряжение с учётом коэффициента
        /// </summary>
        public override double TotalStrain { get; set; }

        /// <summary>
        /// Напряжение, которое эмулирует равномерное скопление машин в пробке
        /// Применяется не на всех нагрузках
        /// </summary>
        public TrafficJamStrain? TrafficJamStrain { get; set; }

        /// <summary>
        /// Траектория, на которой находилось ТС при замере напряжения.
        /// </summary>
        public required VehicleTrajectory VehicleTrajectoryRef { get; set; }

        /// <summary>
        /// массив напряжений индивидуальных ТС
        /// </summary>
        public required VehicleStrain[] VehicleStrains { get; set; }

        /// <summary>
        /// Понижающий коэффициент полосности. Применяется при задействовании многих полос движения одновременно
        /// </summary>
        public double StripeCoefficient { get; set; } = 1d;

        /// <summary>
        /// Понижающий коэффициент полосности. Применяется при задействовании многих полос движения одновременно
        /// </summary>
        public double? TrafficJamStripeCoefficient { get; set; }
    }
}
