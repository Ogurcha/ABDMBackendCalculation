using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public class NoLimitCondition : IPassTypeCondition
    {
        public bool CanPassCondition(List<StrainResult> columnList, SurfaceModel surface)
        {
            var dynamicLoad = columnList.Max(c => c.Strain);

            dynamicLoad *= PassTypeCalculationCoordinator.DynamicCoefficient;

            return surface.MyStrength > surface.ConstLoad + surface.PedestrianLoad + surface.OtherLoad + dynamicLoad;
        }
    }
}
