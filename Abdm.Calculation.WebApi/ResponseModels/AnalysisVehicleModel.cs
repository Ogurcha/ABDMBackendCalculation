using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels
{
    public class AnalysisVehicleModel
    {
        [JsonPropertyName("axles")]
        public required List<AnalysisWheelModel> Axles { get; set; }

        [JsonPropertyName("intervals")]
        public List<AnalysisPositiveIntervalModel>? AnalysisPositiveIntervals { get; set; }
    }
}
