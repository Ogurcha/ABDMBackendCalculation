using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.gost2014
{
    /// <summary>
    /// ГОСТ 32960-2014, приложение А — нагрузка НК.
    /// γf = 1,1; (1+μ) = 1,0; полосность не применяется;
    /// PopulatorCoefficient = 0,75 для сдвоенной НК (п. 4.4.4).
    /// </summary>
    public class NkCoefficientProvider : CoefficientProvider
    {
        public NkCoefficientProvider()
        {
            WorksInSnips = [SnipEnum.gost2014];
            WorksForLoads =
            [
                LoadGroupTypeEnum.NClass,
                LoadGroupTypeEnum.Single,
                LoadGroupTypeEnum.Track
            ];
        }

        public override double GetBasicCoefficent(double lambda) => 1.1d;

        public override double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType) =>
            1d;

        public override double GetPopulatorCoefficient() => 0.75d;
    }
}
