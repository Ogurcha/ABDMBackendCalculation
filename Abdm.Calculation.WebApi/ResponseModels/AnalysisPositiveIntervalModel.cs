using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels
{
    public class AnalysisPositiveIntervalModel
    {
        [JsonPropertyName("number")]
        public double Number { get; set; }

        [JsonPropertyName("leftIntervalStart")]
        public double LeftIntervalStart { get; set; }

        [JsonPropertyName("leftIntervalEnd")]
        public double LeftIntervalEnd { get; set; }

        [JsonPropertyName("leftIntervalLength")]
        public double LeftIntervalLength { get; set; }

        [JsonPropertyName("leftIntervalZ")]
        public double LeftIntervalZ { get; set; }

        [JsonPropertyName("leftIntervalStrain")]
        public double LeftIntervalStrain { get; set; }

        [JsonPropertyName("rightIntervalStart")]
        public double RightIntervalStart { get; set; }

        [JsonPropertyName("rightIntervalEnd")]
        public double RightIntervalEnd { get; set; }

        [JsonPropertyName("rightIntervalLength")]
        public double RightIntervalLength { get; set; }

        [JsonPropertyName("rightIntervalZ")]
        public double RightIntervalZ { get; set; }

        [JsonPropertyName("rightIntervalStrain")]
        public double RightIntervalStrain { get; set; }
    }
}
