using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services.PassTypes.Conditions
{
    public interface IPassTypeCondition
    {
        bool CanPassCondition(List<StrainResult> columnList, SurfaceModel surfaceModel, double? dynamicCoefficient);
    }
}
