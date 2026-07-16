using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public class BarrierInfoModel
    {
        /// <summary>
        /// Абсолютное положение края ограждения слева
        /// </summary>
        [JsonPropertyName("absolutePositionFarLeft")]
        public decimal AbsolutePositionFarLeft { get; set; }

        /// <summary>
        /// Абсолютное положение левого края ограждения по центру
        /// </summary>
        [JsonPropertyName("absolutePositionMiddleLeft")]
        public decimal? AbsolutePositionMiddleLeft { get; set; }

        /// <summary>
        /// Абсолютное положение правого края ограждения по центру
        /// </summary>
        [JsonPropertyName("absolutePositionMiddleRight")]
        public decimal? AbsolutePositionMiddleRight { get; set; }

        /// <summary>
        /// Абсолютное положение края ограждения справа
        /// </summary>
        [JsonPropertyName("absolutePositionFarRight")]
        public decimal AbsolutePositionFarRight { get; set; }

        /// <summary>
        /// Относительное положение ограждения слева
        /// </summary>
        [JsonPropertyName("positionFarLeft")]
        public decimal PositionFarLeft { get; set; }

        /// <summary>
        /// Относительное положение правого края ограждения по центру
        /// </summary>
        [JsonPropertyName("positionMiddleLeft")]
        public decimal? PositionMiddleLeft { get; set; }

        /// <summary>
        /// Относительное положение правого края ограждения по центру
        /// </summary>
        [JsonPropertyName("positionMiddleRight")]
        public decimal? PositionMiddleRight { get; set; }

        /// <summary>
        /// Относительное положение ограждения слева
        /// </summary>
        [JsonPropertyName("positionFarRight")]
        public decimal PositionFarRight { get; set; }

        /// <summary>
        /// Есть ли ограждение на мосту посередине
        /// </summary>
        [JsonPropertyName("hasBarrierInTheMiddle")]
        public bool HasBarrierInTheMiddle { get; set; }
    }
}
