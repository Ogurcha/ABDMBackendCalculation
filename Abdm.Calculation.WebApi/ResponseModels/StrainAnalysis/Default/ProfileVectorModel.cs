using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public struct ProfileVectorModel
    {
        [JsonPropertyName("x")]
        public decimal X { get; set; }

        [JsonPropertyName("y")]
        public decimal Y { get; set; }
    }
}
