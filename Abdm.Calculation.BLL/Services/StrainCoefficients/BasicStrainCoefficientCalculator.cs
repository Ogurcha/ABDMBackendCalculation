using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients
{
    public class BasicStrainCoefficientCalculator : AbstractCoefficientCalculator<IMaterial>, ICoefficientCalculator
    {
        public StrainCoefficientTypeEnum StrainCoefficientType => StrainCoefficientTypeEnum.BasicStrain;

        public StrainCalculationGroupTypeEnum[] StrainCalculationTypes =>
        [
            StrainCalculationGroupTypeEnum.Default,
            StrainCalculationGroupTypeEnum.Pillar,
            StrainCalculationGroupTypeEnum.SteelConcrete,
            StrainCalculationGroupTypeEnum.Slab,
        ];

        public override double GetCoefficient(double lambda, LoadGroupTypeEnum loadGroupType, IMaterial? material)
        {
            switch (loadGroupType)
            {
                case LoadGroupTypeEnum.Common:
                case LoadGroupTypeEnum.AClass:
                    return lambda <= 0
                        ? NormConstants.MaxStrainCoefficient
                        : lambda >= 30
                            ? 1.2
                            : 1.2 + 0.01 * (30 - lambda)
                          ;
                case LoadGroupTypeEnum.Single:
                case LoadGroupTypeEnum.NClass:
                case LoadGroupTypeEnum.Track:
                    return 1d;
                case LoadGroupTypeEnum.AB:
                    return 1.2d;
                default:
                    return NormConstants.MinStrainCoefficient;
            }
        } 
    }
}
