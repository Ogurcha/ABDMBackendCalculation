using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Pillar
{
    public class TrafficJamStrainAnalysisSlimModel
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("intervalStart")]
        public decimal IntervalStart { get; set; }

        [JsonPropertyName("intervalEnd")]
        public decimal IntervalEnd { get; set; }

        [JsonPropertyName("intervalLength")]
        public decimal IntervalLength { get; set; }

        [JsonPropertyName("sumStrain")]
        public decimal SumStrain { get; set; }
    }
}
