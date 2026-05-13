using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services.PassTypes.Conditions
{
    public class NoLimitCondition : IPassTypeCondition
    {
        public bool CanPassCondition(IList<StrainResult> strainResults, SurfaceModel surface, double? dynamicCoefficient)
        {
            return strainResults.GroupBy(x =>
            (PedestrianLoad: x.RoadRuleRef.IsPedestrianAllowed ? surface.PedestrianLoad : 0d,
            x.RoadRuleRef.IsDynamicMovement)).Select(x =>
            {
                var load = x.Max(c => c.Strain.TotalStrain);
                if (x.Key.IsDynamicMovement && dynamicCoefficient is double coeff)
                {
                    load *= coeff;
                }
                return surface.MyStrength > surface.ConstLoad + x.Key.PedestrianLoad + surface.OtherLoad + load;
            }).All(succeded => succeded);
        }
    }
}
