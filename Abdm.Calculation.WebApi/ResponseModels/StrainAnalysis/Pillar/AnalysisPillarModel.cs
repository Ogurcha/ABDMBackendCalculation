using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Pillar
{
    public class AnalysisPillarModel
    {
        [JsonPropertyName("columnNumber")]
        public int ColumnNumber { get; set; }

        [JsonPropertyName("axles")]
        public required List<AxleAnalysisModel> Axles { get; set; }

        [JsonPropertyName("intervals")]
        public List<TrafficJamStrainAnalysisSlimModel>? Intervals { get; set; }
    }
}
