using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients
{
    public abstract class CoefficientProvider : ICoefficientProvider
    {
        public virtual SnipEnum[] WorksInSnips { get; set; } = [];

        public virtual LoadGroupTypeEnum[] WorksForLoads { get; set; } = [];

        public virtual ITrafficJamStrainCoefficientProvider? TrafficJamStrainCoefficientProvider { get; set; } = null;

        public virtual double GetBasicCoefficent(double lambda) => 1d;

        public virtual double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType) => 1d;

        public virtual double[] GetStripeCoefficient(double lambda) => [1d, 1d, 1d, 1d, 1d];

        /// <summary>
        /// Коэффициент при более чем одном ТС в траектории. По умолчанию 1.
        /// </summary>
        public virtual double GetPopulatorCoefficient() => 1d;
    }
}
