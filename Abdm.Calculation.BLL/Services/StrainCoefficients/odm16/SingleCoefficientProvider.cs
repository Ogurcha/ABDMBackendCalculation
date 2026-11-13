using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.odm16
{
    public class SingleCoefficientProvider : CoefficientProvider
    {
        public SingleCoefficientProvider()
        {
            WorksInSnips = [
                SnipEnum.odm16
            ];
            WorksForLoads = [
                LoadGroupTypeEnum.Single,
                LoadGroupTypeEnum.Track,
            ];
        }

        public override double GetBasicCoefficent(double lambda) =>
            1.1d;

        public override double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType) =>
            1.1d;
    }
}
