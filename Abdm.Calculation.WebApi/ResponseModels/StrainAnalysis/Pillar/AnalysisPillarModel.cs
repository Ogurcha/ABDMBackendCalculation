using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Pillar
{
    public class AnalysisPillarModel
    {
        [JsonPropertyName("columnNumber")]
        public int ColumnNumber { get; set; }

        [JsonPropertyName("axles")]
        public required AxleAnalysisModel[] Axles { get; set; }

        [JsonPropertyName("intervals")]
        public TrafficJamStrainAnalysisSlimModel[]? Intervals { get; set; }
    }
}
