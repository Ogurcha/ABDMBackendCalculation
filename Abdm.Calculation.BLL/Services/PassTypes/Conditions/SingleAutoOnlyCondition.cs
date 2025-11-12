using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public class SingleAutoOnlyCondition : IPassTypeCondition
    {
        public bool CanPassCondition(List<StrainResult> columnList, SurfaceModel surface, double? dynamicCoefficient)
        {
            return columnList.Select(x =>
            {
                var load = x.StrainOneAuto;
                return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + load;
            }).All(succeded => succeded);
        }
    }
}
