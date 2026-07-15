using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class CorrectionCoefficients
    {
        [JsonPropertyName("theta")]
        public required string Theta { get; set; }

        [JsonPropertyName("ash3")]
        public required string Ash3 { get; set; }

        [JsonPropertyName("m1")]
        public required string M1 { get; set; }

        [JsonPropertyName("ash4")]
        public required string Ash4 { get; set; }
    }
}
