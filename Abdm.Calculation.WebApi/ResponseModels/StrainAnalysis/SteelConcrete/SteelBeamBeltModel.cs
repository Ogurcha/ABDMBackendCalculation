using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class SteelBeamBeltModel
    {
        [JsonPropertyName("AK")]
        public decimal AK { get; set; }

        [JsonPropertyName("strainAK")]
        public decimal StrainAK { get; set; }

        [JsonPropertyName("limitsAK")]
        public decimal LimitsAK { get; set; }

        [JsonPropertyName("reserveAK")]
        public decimal ReserveAK { get; set; }

        [JsonPropertyName("NK")]
        public decimal NK { get; set; }

        [JsonPropertyName("strainNK")]
        public decimal StrainNK { get; set; }

        [JsonPropertyName("limitsNK")]
        public decimal LimitsNK { get; set; }

        [JsonPropertyName("reserveNK")]
        public decimal ReserveNK { get; set; }

        [JsonPropertyName("N3")]
        public decimal N3 { get; set; }

        [JsonPropertyName("strainN3")]
        public decimal StrainN3 { get; set; }

        [JsonPropertyName("limitsN3")]
        public decimal LimitsN3 { get; set; }

        [JsonPropertyName("reserveN3")]
        public decimal ReserveN3 { get; set; }
    }
}
