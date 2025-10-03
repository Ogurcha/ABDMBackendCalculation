using Abdm.Calculation.BLL.Entities;
using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Колонна - это колонна транспортных средств.
    /// Представляет собой набор потенциальных траекторий 
    /// движения транспортных средств 
    /// внутри некоего интервала
    /// </summary>
    public class ColumnModel 
    {
        /// <summary>
        /// Траектории движения транспортных средств
        /// </summary>
        public VehicleTrajectory[]? VehicleTrajectories { get; set; }

        /// <summary>
        /// Максимальное напряжение по каждой траектории движения
        /// </summary>
        public double[]? Strain { get; set; }

        /// <summary>
        /// Максимальное напряжение по каждой траектории движения, если проезжает по 1 авто
        /// Необходимо для случая проверки <see cref="PassTypeEnum.SingleAutoOnly"/>
        /// </summary>
        public double[]? StrainOneAuto { get; set; }

    }
}
