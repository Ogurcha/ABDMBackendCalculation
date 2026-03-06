using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public class WheelAnalysisModel
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("subNumber")]
        public int SubNumber { get; set; }

        /// <summary>
        /// Вес колеса
        /// </summary>
        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }

        /// <summary>
        /// Ширина отпечатка колеса
        /// </summary>
        [JsonPropertyName("width")]
        public decimal Width { get; set; }

        /// <summary>
        /// Длина отпечатка колеса
        /// </summary>
        [JsonPropertyName("height")]
        public decimal Height { get; set; }

        /// <summary>
        /// Давление колеса на поверхность
        /// </summary>
        [JsonPropertyName("pressure")]
        public decimal Pressure { get; set; }

        [JsonPropertyName("positionX")]
        public decimal PositionX { get; set; }

        [JsonPropertyName("positionY")]
        public decimal PositionY { get; set; }

        [JsonPropertyName("strain")]
        public decimal Strain { get; set; }

        [JsonPropertyName("z")]
        public decimal Z { get; set; }
    }
}
