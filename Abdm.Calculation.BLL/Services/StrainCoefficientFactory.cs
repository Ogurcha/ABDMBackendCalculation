using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCoefficientFactory(IList<ICoefficientCalculator> calculators) : IStrainCoefficientFactory
    {
        public ICoefficientCalculator? GetStrainCalculator(StrainCoefficientTypeEnum strainCoefficientType,
            StrainCalculationGroupTypeEnum strainCalculationType)
            => calculators.Last(s =>
            s.StrainCoefficientType == strainCoefficientType
            && s.StrainCalculationTypes.Contains(strainCalculationType));
    }
}
