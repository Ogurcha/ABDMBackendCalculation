using Abdm.Calculation.DAL.Entities;
using Abdm.Calculation.Graphics.Entities;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Колонна - это колонна транспортных средств.
    /// Представляет собой набор потенциальных траекторий 
    /// движения транспортных средств 
    /// внутри заданного интервала
    /// </summary>
    public class Column 
    {
        public Column(PassageInterval interval)
        {
            Interval = interval;
        }

        /// <summary>
        /// Интервал по которому движется колонна
        /// </summary>
        public PassageInterval Interval { get; set; }

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
