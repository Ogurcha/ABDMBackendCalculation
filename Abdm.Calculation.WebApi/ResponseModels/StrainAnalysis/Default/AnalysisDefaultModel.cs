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

        [JsonPropertyName("barrierPositionFarLeft")]
        public decimal BarrierPositionFarLeft { get; set; }

        [JsonPropertyName("barrierPositionMiddleLeft")]
        public decimal? BarrierPositionMiddleLeft { get; set; }

        [JsonPropertyName("barrierPositionMiddleRight")]
        public decimal? BarrierPositionMiddleRight { get; set; }

        [JsonPropertyName("barrierPositionFarRight")]
        public decimal BarrierPositionFarRight { get; set; }


    }
}
