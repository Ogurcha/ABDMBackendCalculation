using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.odm16
{
    public class AutoColumnCoefficientProvider : CoefficientProvider
    {
        public AutoColumnCoefficientProvider()
        {
            WorksInSnips = [
                SnipEnum.odm16
            ];
            WorksForLoads = [
                LoadGroupTypeEnum.AClass,
                LoadGroupTypeEnum.Common
            ];
            TrafficJamStrainCoefficientProvider = new TrafficJamCoefficientProvider();
        }

        public override double GetBasicCoefficent(double lambda) =>
            Math.Max(1.2d, Math.Min(1.5d, 1.2 + 0.01 * (30 - lambda)));

        public override double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType) =>
            calculationType == StrainCalculationGroupTypeEnum.Pillar
            ? GetDynamicCoefficientPillar(lambda, (PillarMaterial)material)
            : GetDynamicCoefficientBeam(lambda, (SurfaceMaterial)material);

        private double GetDynamicCoefficientBeam(double lambda, SurfaceMaterial material)
        {
            if (material.IsMetal())
            {
                if (material.IsHanging())
                {
                    return 1 + 50 / (70 + lambda);
                }
                else
                {
                    return 1 + 15 / (37.5 + lambda);
                }
            }
            if (material.IsConcrete())
            {
                if (!material.IsArched())
                {
                    return 1 + (45 - lambda) / 135;
                }
                if (material.IsArched() && material.IsSuperArched())
                {
                    return 1 + (70 - lambda) / 250;
                }
            }
            
            return NormConstants.MinStrainCoefficient;
        }

        private double GetDynamicCoefficientPillar(double lambda, PillarMaterial material)
        {
            if (material.IsMetal())
            {
                if (material.IsPylon())
                {
                    return 1 + 50 / (70 + lambda);
                }
                if (!material.IsPylon())
                {
                    return 1 + 15 / (37.5 + lambda);
                }
            }

            return NormConstants.MinStrainCoefficient;
        }
    }
}
