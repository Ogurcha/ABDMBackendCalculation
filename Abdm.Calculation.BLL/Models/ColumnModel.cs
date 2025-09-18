using Abdm.Calculation.BLL.Entities;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Колонна - это колонна транспортных средств.
    /// Представляет собой набор потенциальных траекторий 
    /// движения транспортных средств 
    /// внутри заданного интервала
    /// </summary>
    public class ColumnModel 
    {
        public ColumnModel(PassageIntervalModel interval)
        {
            Interval = interval;
        }

        /// <summary>
        /// Интервал по которому движется колонна
        /// </summary>
        public PassageIntervalModel Interval { get; set; }

        /// <summary>
        /// Координаты по оси X траектрий движения
        /// </summary>
        public double[]? Xs { get; set; }

        /// <summary>
        /// Траектории движения транспортных средств
        /// </summary>
        public SmoothPoints[]? Points { get; set; }

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
