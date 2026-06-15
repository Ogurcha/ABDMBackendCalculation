using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.snip1962
{
    public class TankCoefficientProvider : CoefficientProvider
    {
        public TankCoefficientProvider()
        {
            WorksInSnips = [
                SnipEnum.sn62
            ];
            WorksForLoads = [
                LoadGroupTypeEnum.Single,
                LoadGroupTypeEnum.Track,
                LoadGroupTypeEnum.NClass,
                LoadGroupTypeEnum.AB,
            ];
        }

        public override double GetBasicCoefficent(double lambda) =>
            1.1d;

        public override double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType) => 1d;
    }
}
