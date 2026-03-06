using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Pillar
{
    public class AxleAnalysisModel
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }

        [JsonPropertyName("positionY")]
        public decimal PositionY { get; set; }

        [JsonPropertyName("strain")]
        public decimal Strain { get; set; }

        [JsonPropertyName("z")]
        public decimal Z { get; set; }
    }
}
