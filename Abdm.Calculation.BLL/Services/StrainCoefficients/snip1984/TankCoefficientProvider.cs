using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.snip1984
{
    public class TankCoefficientProvider : CoefficientProvider
    {
        public TankCoefficientProvider()
        {
            WorksInSnips = [
                SnipEnum.sn84
            ];
            WorksForLoads = [
                LoadGroupTypeEnum.Single,
                LoadGroupTypeEnum.Track,
                LoadGroupTypeEnum.NClass,
            ];
        }

        public override double GetBasicCoefficent(double lambda) =>
            1d;

        public override double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType) => 1.1d;
    }
}
