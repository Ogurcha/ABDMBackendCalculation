using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL.Enums;
using Abdm.Calculation.SteelConcrete;
using Mapster;

namespace Abdm.Calculation.BLL.Services.PassTypes
{
    public class SteelConcretePassTypeResolver(ISteelConcretePassChecker steelConcretePassChecker) : IPassTypeResolver
    {
        public StrainCalculationTypeEnum[] StrainCalculationTypes => [StrainCalculationTypeEnum.st40];

        public PassTypeEnum Resolve(List<StrainResult> strainResults, SurfaceModel surface)
        {
            var fullStrain = strainResults.Max(x => x.Strain) * StrainCoefficientFormulas.GetDynamicMovementCoefficient(surface.Lambda);

            if (surface.StrainTypeSpecificData is not SteelConcreteData steelConcreteData)
            {
                return PassTypeEnum.Unknown;
            }

            var passResult = steelConcretePassChecker.CheckPass(fullStrain,
                surface.PedestrianLoad,
                steelConcreteData.CrossSection,
                steelConcreteData.SteelConcreteParameters
                );

            return passResult.Adapt<PassTypeEnum>();
        }
    }
}
