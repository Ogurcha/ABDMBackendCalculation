using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.gost2014
{
    /// <summary>
    /// ГОСТ 32960-2014: равномерно распределённая часть АК — γf = 1,25; (1+μ) = 1,0;
    /// полосность s1=1,0 / s2=0,6 / s3+=0,3.
    /// </summary>
    public class TrafficJamCoefficientProvider : ITrafficJamStrainCoefficientProvider
    {
        public double GetBasicCoefficent(double lambda) => 1.25d;

        public double[] GetStripeCoefficient(double lambda) => [1d, 0.6d, 0.3d, 0.3d, 0.3d];
    }
}
