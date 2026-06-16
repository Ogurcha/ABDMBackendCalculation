using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.snip1938
{
    public class SimpleCoefficientProvider : CoefficientProvider
    {
        public SimpleCoefficientProvider()
        {
            WorksInSnips = [
                SnipEnum.sn38,
            ];
            WorksForLoads = [
                LoadGroupTypeEnum.AClass,
                LoadGroupTypeEnum.Common,
            ];
        }

        public override double GetBasicCoefficent(double lambda) =>
            1d;

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
                    //Это часть моста, которая перекрывает реку, дорогу или овраг и держит проезд. Она сделана из металла или из смеси стали и железобетона.
                    return 1 + 15 / (37.5 + lambda);
                }
                else
                {
                    // Элементы главных ферм металлических пролетных строений и металлических пилонов висячих и вантовых мостов
                    // Это основные несущие “скелеты” металлических мостов и высокие стойки/башни у висячих и вантовых мостов
                    return 1 + 50 / (75 + lambda);
                }
            }
            if (material.IsConcrete())
            {
                if (!material.IsArched())
                {
                    // Железобетонные балочные пролетные строения, рамные конструкции, сквозные надарочные строения
                    // Это мостовые части из железобетона:
                    // балочные — как длинная прочная балка;
                    // рамные — как жёсткая “рама”;
                    // сквозные надарочные — арочные или похожие конструкции, где часть моста “просматривается” сквозь каркас.
                    return Math.Max(1.1, Math.Min(1.2, 1.1 + 0.01 * (15 - lambda)));
                }
                if (material.IsArched() && material.IsSuperArched() && material.SuperStructureType != SuperStructureTypeEnum.ArchesWithAStaircase)
                {
                    // Арки и своды арочных железобетонных пролетных строений со сквозной надарочной конструкцией
                    // Это арочные мосты, где нагрузку несёт арка, а сверху есть открытая конструкция, через которую можно “видеть насквозь”
                    return Math.Max(1, Math.Min(1.15, 1 + 0.003 * (70 - lambda)));
                }
                if (material.IsArched() && material.SuperStructureType == SuperStructureTypeEnum.ArchesWithAStaircase)
                {
                    // Железобетонные, бетонные и каменные арки со сплошным надсводным строением
                    // Это тоже арочные мосты, но сверху у них сплошная масса — не “сквозная”, а цельная.
                    return Math.Max(1, Math.Min(1.1, 1 + 0.002 * (70 - lambda)));
                }
                if (material.IsWood())
                {
                    // Деревянные конструкции пролетных строений
                    return 1d;
                }
                if (material.IsStone())
                {
                    // Массивные опоры (бетонные, каменные), деревянные опоры, фундаменты и основания
                    return 1d;
                }
            }

            return NormConstants.MinStrainCoefficient;
        }

        private double GetDynamicCoefficientPillar(double lambda, PillarMaterial material)
        {
            if (material.IsMetal())
            {
                // Элементы металлических опор кроме пилонов висячих и вантовых мостов
                if (!material.IsPylon())
                {
                    return 1 + 15 / (37.5 + lambda);
                }
                // Элементы металлических пилонов висячих и вантовых мостов
                if (material.IsPylon())
                {
                    return 1 + 15 / (37.5 + lambda);
                }
            } else
            {
                // Железобетонные сквозные, тонкостенные и стоечные опоры
                return 1d;
            }

            return NormConstants.MinStrainCoefficient;
        }

        public override double[] GetStripeCoefficient(double lambda) => [1d, 1d, 0.85d, 0.75d, 0.75d];
    }
}
