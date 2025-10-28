using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPassTypeResolver
    {
        public StrainCalculationTypeEnum[] StrainCalculationTypes { get; }

        PassTypeEnum Resolve(List<StrainResult> strainResults, SurfaceModel surfaceModel);
    }
}