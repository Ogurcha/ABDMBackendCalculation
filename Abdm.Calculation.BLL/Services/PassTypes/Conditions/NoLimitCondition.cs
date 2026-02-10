using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public class NoLimitCondition : IPassTypeCondition
    {
        public bool CanPassCondition(List<StrainResult> columnList, SurfaceModel surface, double? dynamicCoefficient)
        {
            return columnList.GroupBy(x =>
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
