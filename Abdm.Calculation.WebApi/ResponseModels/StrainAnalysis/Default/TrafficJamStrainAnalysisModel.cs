using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public class TrafficJamStrainAnalysisModel
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("leftIntervalStart")]
        public decimal LeftIntervalStart { get; set; }

        [JsonPropertyName("leftIntervalEnd")]
        public decimal LeftIntervalEnd { get; set; }

        [JsonPropertyName("leftIntervalLength")]
        public decimal LeftIntervalLength { get; set; }

        [JsonPropertyName("leftIntervalStrain")]
        public decimal LeftIntervalStrain { get; set; }

        [JsonPropertyName("rightIntervalStart")]
        public decimal RightIntervalStart { get; set; }

        [JsonPropertyName("rightIntervalEnd")]
        public decimal RightIntervalEnd { get; set; }

        [JsonPropertyName("rightIntervalLength")]
        public decimal RightIntervalLength { get; set; }

        [JsonPropertyName("rightIntervalStrain")]
        public decimal RightIntervalStrain { get; set; }

        [JsonPropertyName("sumStrain")]
        public decimal SumStrain { get; set; }

        [JsonPropertyName("centerIntervalStart")]
        public decimal CenterIntervalStart { get; set; }

        [JsonPropertyName("centerIntervalEnd")]
        public decimal CenterIntervalEnd { get; set; }

        [JsonPropertyName("centerIntervalLength")]
        public decimal CenterIntervalLength { get; set; }

        [JsonPropertyName("leftIntervalVolume")]
        public decimal LeftIntervalVolume { get; set; }

        [JsonPropertyName("rightIntervalVolume")]
        public decimal RightIntervalVolume { get; set; }
    }
}
