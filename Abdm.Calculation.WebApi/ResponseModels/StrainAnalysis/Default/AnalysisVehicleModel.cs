using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public class AnalysisVehicleModel
    {
        [JsonPropertyName("columnNumber")]
        public int ColumnNumber { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [JsonPropertyName("vehicleNumber")]
        public int VehicleNumber { get; set; } = 1;

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

        [JsonPropertyName("intervalProfilePoints")]
        public ProfileVectorModel[]? IntervalProfileVectors { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [JsonPropertyName("lambda")]
        public decimal Lambda { get; set; } = 33m;

        /// <summary>
        /// TODO
        /// </summary>
        [JsonPropertyName("partLength")]
        public decimal PartLength { get; set; } = 33m;

        /// <summary>
        /// TODO
        /// </summary>
        [JsonPropertyName("dynamicCoefficient")]
        public decimal DynamicCoefficient { get; set; } = 1.1m;

        /// <summary>
        /// TODO
        /// </summary>
        [JsonPropertyName("totalStrain")]
        public decimal TotalStrain { get; set; } = 83.2m;
    }
}
