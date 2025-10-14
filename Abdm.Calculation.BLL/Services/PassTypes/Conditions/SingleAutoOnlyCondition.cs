using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public class SingleAutoOnlyCondition : IPassTypeCondition
    {
        public bool CanPassCondition(List<StrainResult> columnList, SurfaceModel surface)
        {
            var dynamicLoad = columnList.Sum(c => c.StrainOneAuto);

            return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + dynamicLoad;
        }
    }
}
