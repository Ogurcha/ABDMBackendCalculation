using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public class AnalysisDefaultModel
    {
        [JsonPropertyName("hasSafetyLine")]
        public bool? HasSafetyLine { get; set; }

        [JsonPropertyName("forward")]
        public bool IsForward { get; set; }

        [JsonPropertyName("vehicles")]
        public required AnalysisVehicleModel[] Vehicles { get; set; }
    }
}
