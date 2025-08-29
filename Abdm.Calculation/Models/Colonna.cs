using Abdm.Calculation.Graphics;

namespace Abdm.Calculation.Models
{
    /// <summary>
    /// колонна - это колонна транспортных средств.
    /// </summary>
    public class Colonna 
    {
        public Colonna(PassageInterval interval)
        {
            this.Interval = interval;
        }

        /// <summary>
        /// Интервал по которому движется колонна
        /// </summary>
        public PassageInterval Interval { get; set; }

        /// <summary>
        /// Экстремумы транспортных средств
        /// </summary>
        public SmoothPoints[] Points { get; set; }

        /// <summary>
        /// Напряжения ТС
        /// </summary>
        public double[] Strain { get; set; }

    }
}
