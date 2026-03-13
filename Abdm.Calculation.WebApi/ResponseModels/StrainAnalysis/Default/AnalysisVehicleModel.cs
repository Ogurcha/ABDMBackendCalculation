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
        public required WheelAnalysisModel[] Wheels { get; set; }

        [JsonPropertyName("sumStrain")]
        public decimal SumStrain { get; set; }

        [JsonPropertyName("intervals")]
        public TrafficJamStrainAnalysisModel[]? Intervals { get; set; }
    }
}
