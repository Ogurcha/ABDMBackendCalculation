using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public class Speed10Condition : IPassTypeCondition
    {
        public bool CanPassCondition(List<StrainResult> columnList, SurfaceModel surface)
        {
            var dynamicLoad = columnList.Sum(c => c.Strain);

            return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + dynamicLoad;
        }
    }
}
