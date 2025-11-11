using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients
{
    public class TrafficJamStrainCoefficientCalculator : ICoefficientCalculator<IMaterial>
    {
        public StrainCoefficientTypeEnum StrainCoefficientType => StrainCoefficientTypeEnum.TrafficJam;

        public StrainCalculationGroupTypeEnum[] StrainCalculationTypes =>
        [
            StrainCalculationGroupTypeEnum.Default,
            StrainCalculationGroupTypeEnum.SteelConcrete,
            StrainCalculationGroupTypeEnum.Pillar,
        ];

        public double Get(double lambda, LoadGroupTypeEnum loadGroupType, IMaterial material) => Math.Min(NormConstants.MaxStrainCoefficient, Math.Max(NormConstants.MinStrainCoefficient, GetCoefficient(lambda, loadGroupType, material)));

        private double GetCoefficient(double lambda, LoadGroupTypeEnum loadGroupType, IMaterial material)
        {
            return 1.2d;
        } 
    }
}
