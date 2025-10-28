using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.SteelConcrete;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Services.SteelConcrete
{
    public class SteelConcretePassTypeResolver : IPassTypeResolver
    {
        public StrainCalculationTypeEnum[] StrainCalculationTypes => [StrainCalculationTypeEnum.st40];

        public PassTypeEnum Resolve(List<StrainResult> strainResults, SurfaceModel surface)
        {
            var fullStrain = strainResults.Max(x => x.Strain) * StrainCoefficientFormulas.GetDynamicMovementCoefficient(surface.Lambda);

            if (surface.StrainTypeSpecificData is not SteelConcreteData steelConcreteData)
            {
                return PassTypeEnum.Unknown;
            }


            return PassTypeEnum.NoLimit;
        }
    }
}
