using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.RequestModels
{
    /// <summary>
    /// DTO Информация об осях
    /// </summary>
    public class AxleRequestModel
    {
        [JsonPropertyName("y")]
        public double Y { get; set; }

        [JsonPropertyName("wx")]
        public double Wx { get; set; }

        [JsonPropertyName("wy")]
        public double Wy { get; set; }

        /// <summary>
        /// Вес колеса
        /// </summary>
        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        /// <summary>
        /// Абсолютная длина проекции, с учетом текущего колеса и колёс позади
        /// </summary>
        [JsonPropertyName("absY")]
        public double AbsolutY { get; set; }

        /// <summary>
        /// Габариты колеса (их может быть несколько)
        /// </summary>
        [JsonPropertyName("wheels")]
        public double[]? Wheels { get; set; }
    }
}
