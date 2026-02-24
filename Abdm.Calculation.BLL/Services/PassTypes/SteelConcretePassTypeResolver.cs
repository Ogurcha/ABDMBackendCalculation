using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.SteelConcrete;
using Mapster;

namespace Abdm.Calculation.BLL.Services.PassTypes
{
    public class SteelConcretePassTypeResolver(IStrainCoefficientFactory strainCoefficientFactory,
        ISteelConcretePassChecker steelConcretePassChecker) : IPassTypeResolver
    {
        public StrainCalculationGroupTypeEnum[] StrainCalculationTypes => [StrainCalculationGroupTypeEnum.SteelConcrete];

        public PassTypeEnum Resolve(List<StrainResult> strainResults, VehicleRollingSmallModel data)
        {
            var fullStrain = strainResults.Max(x => x.Strain.TotalStrain);

            if (strainCoefficientFactory.GetStrainCalculator(StrainCoefficientTypeEnum.DynamicMovement, data.Surface.StrainCalculationGroupType) is ICoefficientCalculator calculator)
            {
                fullStrain *= calculator.Get(data.Surface.Lambda, data.Load.Type, data.Surface.Material);
            }

            if (data.Surface.StrainTypeSpecificData is not SteelConcreteData steelConcreteData)
            {
                return PassTypeEnum.Unknown;
            }
            
            var passResult = steelConcretePassChecker.CheckPass(fullStrain,
                data.Surface.PedestrianLoad,
                steelConcreteData.CrossSection,
                steelConcreteData.SteelConcreteParameters!
                );

            return passResult.Adapt<PassTypeEnum>();
        }
    }
}
