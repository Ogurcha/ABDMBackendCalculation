using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.SteelConcrete;
using Mapster;

namespace Abdm.Calculation.BLL.Services.PassTypes
{
    public class SteelConcretePassTypeResolver(ISteelConcretePassChecker steelConcretePassChecker) : IPassTypeResolver
    {
        public StrainCalculationGroupTypeEnum[] StrainCalculationTypes => [StrainCalculationGroupTypeEnum.SteelConcrete];

        public PassTypeEnum Resolve(IList<StrainResult> strainResults, VehicleRollingSmallModel data)
        {
            var fullStrain = strainResults.Max(x => x.TotalStrain) * data.DynamicCoefficient();

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
