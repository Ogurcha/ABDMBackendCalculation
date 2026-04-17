using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Pillar
{
    public class AnalysisPillarModel
    {
        [JsonPropertyName("columnNumber")]
        public int ColumnNumber { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [JsonPropertyName("vehicleNumber")]
        public int VehicleNumber { get; set; } = 1;

        /// <summary>
        /// TODO
        /// </summary>
        [JsonPropertyName("positionX")]
        public decimal PositionX { get; set; }

        [JsonPropertyName("axles")]
        public required AxleAnalysisModel[] Axles { get; set; }

        [JsonPropertyName("intervals")]
        public TrafficJamStrainAnalysisSlimModel[]? Intervals { get; set; }

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

        [JsonPropertyName("forward")]
        public bool IsForward { get; set; }
    }
}
