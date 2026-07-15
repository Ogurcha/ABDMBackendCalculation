using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class CorrectionCoefficientsModel
    {
        [JsonPropertyName("theta")]
        public string Theta { get; set; }

        [JsonPropertyName("ash3")]
        public string Ash3 { get; set; }

        [JsonPropertyName("m1")]
        public string M1 { get; set; }

        [JsonPropertyName("ash4")]
        public string Ash4 { get; set; }
    }
}
