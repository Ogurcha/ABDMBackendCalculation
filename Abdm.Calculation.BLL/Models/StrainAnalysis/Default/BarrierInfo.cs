namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class BarrierInfo
    {   
        /// <summary>
        /// Абсолютное положение края ограждения слева
        /// </summary>
        public decimal AbsolutePositionFarLeft { get; set; }

        /// <summary>
        /// Абсолютное положение левого края ограждения по центру
        /// </summary>
        public decimal? AbsolutePositionMiddleLeft { get; set; }

        /// <summary>
        /// Абсолютное положение правого края ограждения по центру
        /// </summary>
        public decimal? AbsolutePositionMiddleRight { get; set; }

        /// <summary>
        /// Абсолютное положение края ограждения справа
        /// </summary>
        public decimal AbsolutePositionFarRight { get; set; }

        /// <summary>
        /// Относительное положение ограждения слева
        /// </summary>
        public decimal PositionFarLeft { get; set; }

        /// <summary>
        /// Относительное положение правого края ограждения по центру
        /// </summary>
        public decimal? PositionMiddleLeft { get; set; }

        /// <summary>
        /// Относительное положение правого края ограждения по центру
        /// </summary>
        public decimal? PositionMiddleRight { get; set; }

        /// <summary>
        /// Относительное положение ограждения слева
        /// </summary>
        public decimal PositionFarRight { get; set; }

        /// <summary>
        /// Есть ли ограждение на мосту посередине
        /// </summary>
        public bool HasBarrierInTheMiddle { get; set; }
    }
}
