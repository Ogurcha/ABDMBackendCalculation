using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class ConcreteStressModel
    {
        [JsonPropertyName("stressInConcrete")]
        public decimal StressInConcrete { get; set; }

        [JsonPropertyName("stressInArmature")]
        public decimal StressInArmature { get; set; }
    }
}
