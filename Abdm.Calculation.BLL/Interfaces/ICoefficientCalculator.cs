using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ICoefficientCalculator<T> : ICoefficientCalculator where T : IMaterial 
    {
        public double Get(double lambda, LoadGroupTypeEnum loadGroupType, T material);
    }

    public interface ICoefficientCalculator
    {
        StrainCalculationGroupTypeEnum[] StrainCalculationTypes { get; }

        StrainCoefficientTypeEnum StrainCoefficientType { get; }
    }
}