using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.snip1984
{
    public class MediumCoefficientProvider : CoefficientProvider
    {
        public MediumCoefficientProvider()
        {
            WorksInSnips = [
                SnipEnum.sn84
            ];
            WorksForLoads = [
                LoadGroupTypeEnum.AClass,
                LoadGroupTypeEnum.Common,                 
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
                if (!material.IsHanging())
                {
                    // Металлические и сталежелезобетонные пролетные строения всех систем, кроме элементов главных ферм висячих и вантовых мостов
                    return 1 + 15 / (37.5 + lambda);
                }

                // Элементы главных ферм металлических пролетных строений и металлических пилонов висячих и вантовых мостов
                return 1 + 50 / (70 + lambda);
            }
            if (material.IsConcrete())
            {
                if (!material.IsArched())
                {
                    // Железобетонные балочные пролетные строения, рамные конструкции, сквозные надарочные строения (СНиП 2.05.03-84)
                    return Math.Max(1d, 1 + (45 - lambda) / 135);
                }
                if (material.IsArched() && material.IsSuperArched() && material.SuperStructureType != SuperStructureTypeEnum.ArchesWithAStaircase)
                {
                    // Арки и своды арочных железобетонных пролетных строений со сквозной надарочной конструкцией
                    return Math.Max(1d, 1 + (70 - lambda) / 250);
                }
                if (material.IsArched() && material.SuperStructureType == SuperStructureTypeEnum.ArchesWithAStaircase)
                {
                    // Железобетонные, бетонные и каменные арки со сплошным надсводным строением
                    return 1d;
                }
            }
            if (material.IsWood() || material.IsStone())
            {
                return 1d;
            }

            return NormConstants.MinStrainCoefficient;
        }

        private double GetDynamicCoefficientPillar(double lambda, PillarMaterial material)
        {
            if (material.IsMetal())
            {
                // Элементы металлических пилонов висячих и вантовых мостов
                if (material.IsPylon())
                {
                    return 1 + 50 / (70 + lambda);
                }

                // Элементы металлических опор кроме пилонов висячих и вантовых мостов
                return 1 + 15 / (37.5 + lambda);
            }

            // Массивные опоры (бетонные, каменные), деревянные опоры, фундаменты и основания
            if (material.IsMassiveOrTimberSupport())
            {
                return 1d;
            }

            // Железобетонные сквозные, тонкостенные и стоечные опоры
            return Math.Max(1d, 1 + (45 - lambda) / 135);
        }
    }
}
