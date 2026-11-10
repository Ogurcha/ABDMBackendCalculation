using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.odm16
{
    public class NClassCoefficientProvider : CoefficientProvider
    {
        public NClassCoefficientProvider()
        {
            WorksInSnips = [
                SnipEnum.odm16,
                SnipEnum.gost2014
            ];
            WorksForLoads = [
                LoadGroupTypeEnum.NClass,
            ];
        }

        public override double GetBasicCoefficent(double lambda) =>
            1d;

        public override double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType) =>
            1.1d;

        /// <summary>
        /// ГОСТ 32960-2014 п. 4.4.4 — понижающий коэффициент для сдвоенной НК.
        /// Для odm16 MaxVehicleInTrajectory = 1, поэтому коэффициент не применяется.
        /// </summary>
        public override double GetPopulatorCoefficient() => 0.75d;
    }
}