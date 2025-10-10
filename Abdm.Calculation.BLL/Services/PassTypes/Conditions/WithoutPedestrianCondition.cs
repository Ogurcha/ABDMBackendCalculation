using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public class WithoutPedestrianCondition : IPassTypeCondition
    {
        public bool CanPassCondition(List<StrainResult> columnList, Surface surface)
        {
            var dynamicLoad = columnList.Sum(c => c.Strain);

            dynamicLoad *= PassTypeCalculationCoordinator.DynamicCoefficient;

            return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + dynamicLoad;
        }
    }
}
