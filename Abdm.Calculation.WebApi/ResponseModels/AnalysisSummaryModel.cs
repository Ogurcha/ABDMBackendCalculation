using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels
{
    public class AnalysisSummaryModel
    {
        [JsonPropertyName("vehicles")]
        public required List<AnalysisVehicleModel> Vehicles { get; set; }


    }
}
