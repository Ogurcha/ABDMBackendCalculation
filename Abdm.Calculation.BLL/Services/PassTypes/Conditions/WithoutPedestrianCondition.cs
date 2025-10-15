using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public class WithoutPedestrianCondition : IPassTypeCondition
    {
        public bool CanPassCondition(List<StrainResult> columnList, SurfaceModel surface)
        {
            return columnList.GroupBy(x =>
            x.RoadRuleRef.IsDynamicMovement).Select(x =>
            {
                var load = x.Max(c => c.Strain);
                if (x.Key)
                {
                    load *= PassTypeCalculationCoordinator.DynamicCoefficient;
                }
                return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + load;
            }).All(succeded => succeded);
        }
    }
}
