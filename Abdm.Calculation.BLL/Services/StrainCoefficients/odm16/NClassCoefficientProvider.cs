using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.odm16
{
    public class NClassCoefficientProvider : CoefficientProvider
    {
        public NClassCoefficientProvider()
        {
            WorksInSnips = [
                SnipEnum.odm16
            ];
            WorksForLoads = [
                LoadGroupTypeEnum.NClass,
            ];
        }

        public override double GetBasicCoefficent(double lambda) =>
            1d;

        public override double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType) =>
            1.1d;
    }
}