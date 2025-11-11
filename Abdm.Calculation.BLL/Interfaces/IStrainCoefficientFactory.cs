using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainCoefficientFactory
    {
        ICoefficientCalculator? GetStrainCalculator(StrainCoefficientTypeEnum strainCoefficientType, StrainCalculationGroupTypeEnum strainCalculationType);
    }
}