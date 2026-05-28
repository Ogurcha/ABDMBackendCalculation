using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public class AnalysisDefaultModel
    {
        [JsonPropertyName("hasSafetyLine")]
        public bool? HasSafetyLine { get; set; }

        [JsonPropertyName("forward")]
        public bool IsForward { get; set; }

        [JsonPropertyName("columns")]
        public required AnalysisColumnModel[] Columns { get; set; }

        [JsonPropertyName("barrierPositionLeft")]
        public decimal BarrierPositionLeft { get; set; }

        [JsonPropertyName("barrierPositionRight")]
        public decimal BarrierPositionRight { get; set; }
    }
}
