using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ICoefficientProvider : ITrafficJamStrainCoefficientProvider
    {
        SnipEnum[] WorksInSnips { get; set; }

        LoadGroupTypeEnum[] WorksForLoads { get; set; }

        ITrafficJamStrainCoefficientProvider? TrafficJamStrainCoefficientProvider { get; set; }

        double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType);
    }

    public interface ITrafficJamStrainCoefficientProvider
    {
        double GetBasicCoefficent(double lambda);

        double[] GetStripeCoefficient(double lambda);
    }
}
