using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ICoefficientCalculator
    {
        StrainCalculationGroupTypeEnum[] StrainCalculationTypes { get; }

        StrainCoefficientTypeEnum StrainCoefficientType { get; }

        public double Get(double lambda, LoadGroupTypeEnum loadGroupType, IMaterial? material);
    }
}