using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public class AnalysisColumnModel
    {
        [JsonPropertyName("columnNumber")]
        public int ColumnNumber { get; set; }

        [JsonPropertyName("vehicleNumber")]
        public int VehicleNumber { get; set; }

        [JsonPropertyName("positionX")]
        public decimal PositionX { get; set; }

        [JsonPropertyName("positionY")]
        public decimal PositionY { get; set; }

        [JsonPropertyName("positionYForImage")]
        public decimal PositionYForImage { get; internal set; }

        [JsonPropertyName("wheels")]
        public required WheelAnalysisModel[] Wheels { get; set; }

        [JsonPropertyName("sumStrain")]
        public decimal SumStrain { get; set; }

        [JsonPropertyName("totalStrain")]
        public decimal TotalStrain { get; set; }

        [JsonPropertyName("intervals")]
        public TrafficJamStrainAnalysisModel[]? Intervals { get; set; }

        [JsonPropertyName("intervalProfilePoints")]
        public ProfileVectorModel[]? IntervalProfileVectors { get; set; }

        [JsonPropertyName("lambdaSmall")]
        public decimal LambdaSmall { get; set; }

        [JsonPropertyName("dynamicCoefficient")]
        public decimal DynamicCoefficient { get; set; }
    }
}
