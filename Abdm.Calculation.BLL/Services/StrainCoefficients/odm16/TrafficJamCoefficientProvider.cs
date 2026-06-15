using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.odm16
{
    public class TrafficJamCoefficientProvider : ITrafficJamStrainCoefficientProvider
    {
        public double GetBasicCoefficent(double lambda) => 1.2d;

        public double[] GetStripeCoefficient(double lambda) => [1d, 0.6d, 0.6d, 0.6d, 0.6d];
    }
}
