using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class SteelBeamBeltsModel
    {
        [JsonPropertyName("upperBelt")]
        public SteelBeamBeltModel UpperBelt { get; set; }

        [JsonPropertyName("lowerBelt")]
        public SteelBeamBeltModel LowerBelt { get; set; }
    }
}
