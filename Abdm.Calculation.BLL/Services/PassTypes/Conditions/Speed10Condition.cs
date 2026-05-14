using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services.PassTypes.Conditions
{
    public class Speed10Condition : IPassTypeCondition
    {
        public bool CanPassCondition(IList<StrainResult> strainResults, SurfaceModel surface, double? dynamicCoefficient)
        {
            return strainResults.Select(x =>
            {
                var load = x.TotalStrain;
                return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + load;
            }).All(succeded => succeded);
        }
    }
}
