using System.Collections.Generic;
using Abdm.Calculation.Graphics;

namespace Abdm.Calculation.Models
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
            this.Interval = interval;
        }

        /// <summary>
        /// Интервал по которому движется колонна
        /// </summary>
        public PassageInterval Interval { get; set; }

        /// <summary>
        /// Координаты по оси X траектрий движения
        /// </summary>
        public double[] Xs { get; set; }

        /// <summary>
        /// Траектории движения транспортных средств
        /// </summary>
        public SmoothPoints[] Points { get; set; }

        /// <summary>
        /// Максимальное напряжение по каждой траектории движения
        /// </summary>
        public double[] Strain { get; set; }

    }
}
