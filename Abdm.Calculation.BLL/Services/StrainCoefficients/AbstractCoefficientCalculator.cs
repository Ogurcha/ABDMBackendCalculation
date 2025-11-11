using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients
{
    public abstract class AbstractCoefficientCalculator<T> where T : class, IMaterial
    {
        public virtual double Get(double lambda, LoadGroupTypeEnum loadGroupType, IMaterial? material) => 
            Math.Min(NormConstants.MaxStrainCoefficient, 
                Math.Max(NormConstants.MinStrainCoefficient, 
                    GetCoefficient(lambda, loadGroupType, material as T)
                    )
                );

        public abstract double GetCoefficient(double lambda, LoadGroupTypeEnum loadGroupType, T? material);
    }
}
