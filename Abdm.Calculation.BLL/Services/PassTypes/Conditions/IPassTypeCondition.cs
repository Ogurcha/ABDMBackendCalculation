using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services.PassTypes.Conditions
{
    public interface IPassTypeCondition
    {
        bool CanPassCondition(IList<StrainResult> strainResults, SurfaceModel surfaceModel, double? dynamicCoefficient);
    }
}
