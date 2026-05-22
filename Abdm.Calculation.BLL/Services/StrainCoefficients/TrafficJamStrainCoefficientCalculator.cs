using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients
{
    public class TrafficJamStrainCoefficientCalculator : AbstractCoefficientCalculator<IMaterial>, ICoefficientCalculator
    {
        public StrainCoefficientTypeEnum StrainCoefficientType => StrainCoefficientTypeEnum.TrafficJam;

        public StrainCalculationGroupTypeEnum[] StrainCalculationTypes =>
        [
            StrainCalculationGroupTypeEnum.Default,
            StrainCalculationGroupTypeEnum.SteelConcrete,
            StrainCalculationGroupTypeEnum.Pillar,
            StrainCalculationGroupTypeEnum.Slab,
        ];

        public override double GetCoefficient(double lambda, LoadGroupTypeEnum loadGroupType, IMaterial? material)
        {
            return 1.2d;
        } 
    }
}
