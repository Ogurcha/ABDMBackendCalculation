using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public class WithoutPedestrianCondition : IPassTypeCondition
    {
        public bool CanPassCondition(List<StrainResult> columnList, SurfaceModel surface, double? dynamicCoefficient)
        {
            return columnList.GroupBy(x =>
            x.RoadRuleRef.IsDynamicMovement).Select(x =>
            {
                var load = x.Max(c => c.Strain.TotalStrain);
                if (x.Key && dynamicCoefficient is double coeff)
                {
                    load *= coeff;
                }
                return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + load;
            }).All(succeded => succeded);
        }
    }
}
