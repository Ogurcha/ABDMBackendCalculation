using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public class Speed10Condition : IPassTypeCondition
    {
        public bool CanPassCondition(List<StrainResult> columnList, SurfaceModel surface)
        {
            return columnList.Select(x =>
            {
                var load = x.Strain;
                return surface.MyStrength > surface.ConstLoad + surface.OtherLoad + load;
            }).All(succeded => succeded);
        }
    }
}
