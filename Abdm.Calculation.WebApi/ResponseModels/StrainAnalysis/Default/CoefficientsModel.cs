using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default
{
    public class CoefficientsModel
    {
        [JsonPropertyName("stripe")]
        public decimal Stripe { get; set; }

        [JsonPropertyName("dynamic")]
        public decimal Dynamic { get; set; }

        [JsonPropertyName("reliability")]
        public decimal Reliability { get; set; }

        [JsonPropertyName("stripeInterval")]
        public decimal? StripeInterval { get; set; }

        [JsonPropertyName("dynamicInterval")]
        public decimal? DynamicInterval { get; set; }

        [JsonPropertyName("reliabilityInterval")]
        public decimal? ReliabilityInterval { get; set; }
    }
}
