using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public class AnalysisVehicleModel
    {
        [JsonPropertyName("columnNumber")]
        public int ColumnNumber { get; set; }

        [JsonPropertyName("positionX")]
        public decimal PositionX { get; set; }

        [JsonPropertyName("positionY")]
        public decimal PositionY { get; set; }

        [JsonPropertyName("wheels")]
        public required List<WheelAnalysisModel> Wheels { get; set; }

        [JsonPropertyName("sumStrain")]
        public decimal SumStrain { get; set; }

        [JsonPropertyName("intervals")]
        public List<TrafficJamStrainAnalysisModel>? Intervals { get; set; }
    }
}
