using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models
{
    public class PassageInterval
    {
        /// <summary>
        /// Общая ширина интервала
        /// </summary>
        public double TotalWidth { get; set; }

        /// <summary>
        /// Абсолютное положение начала интервала
        /// </summary>
        public double AbsolutePositionLeft { get; set; }

        /// <summary>
        /// Абсолютное положение конца интервала
        /// </summary>
        public double AbsolutePositionRight { get; set; }

        /// <summary>
        /// Длина полосы безопасности слева
        /// </summary>
        public double SafetyLineLeft { get; set; }

        /// <summary>
        /// Длина полосы безопасности справа
        /// </summary>
        public double SafetyLineRight { get; set; }

        /// <summary>
        /// Количество полос движения на данном интервале
        /// </summary>
        public int LaneCount { get; set; }

        /// <summary>
        /// Тип движения на интервале
        /// </summary>
        public PassageIntervalTypeEnum Type { get; set; }
    }
}