using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels
{
    public class AnalysisWheelModel
    {
        [JsonPropertyName("number")]
        public double Number { get; set; }

        [JsonPropertyName("SubNumber")]
        public double SubNumber { get; set; }

        [JsonPropertyName("weight")]
        /// <summary>
        /// Вес колеса
        /// </summary>
        public double Weight { get; set; }

        [JsonPropertyName("width")]
        /// <summary>
        /// Ширина отпечатка колеса
        /// </summary>
        public double Width { get; set; }

        [JsonPropertyName("height")]
        /// <summary>
        /// Длина отпечатка колеса
        /// </summary>
        public double Height { get; set; }

        [JsonPropertyName("pressure")]
        /// <summary>
        /// Давление колеса на поверхность
        /// </summary>
        public double Pressure { get; set; }

        [JsonPropertyName("positionX")]
        public double PositionX { get; set; }

        [JsonPropertyName("positionY")]
        public double PositionY { get; set; }

        [JsonPropertyName("strain")]
        public double Strain { get; set; }

        [JsonPropertyName("z")]
        public double Z { get; set; }
    }
}
