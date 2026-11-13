using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.gost2014
{
    /// <summary>
    /// ГОСТ 32960-2014, приложение А — нагрузка АК (тележка).
    /// γf = 1,5; полосность 1,0 / 0,6 / 0,3; (1+μ) по типу конструкции (п. А.1.2).
    /// </summary>
    public class AkCoefficientProvider : CoefficientProvider
    {
        public AkCoefficientProvider()
        {
            WorksInSnips = [SnipEnum.gost2014];
            WorksForLoads =
            [
                LoadGroupTypeEnum.AClass,
                LoadGroupTypeEnum.Common
            ];
            TrafficJamStrainCoefficientProvider = new TrafficJamCoefficientProvider();
        }

        public override double GetBasicCoefficent(double lambda) => 1.5d;

        public override double[] GetStripeCoefficient(double lambda) => [1d, 0.6d, 0.3d, 0.3d, 0.3d];

        public override double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType)
        {
            if (calculationType == StrainCalculationGroupTypeEnum.Pillar)
            {
                return GetDynamicCoefficientPillar((PillarMaterial)material);
            }

            // Элементы проезжей части
            if (calculationType == StrainCalculationGroupTypeEnum.Slab)
            {
                return 1.4d;
            }

            return GetDynamicCoefficientBeam((SurfaceMaterial)material);
        }

        private static double GetDynamicCoefficientBeam(SurfaceMaterial material)
        {
            if (material.IsWood())
            {
                return 1d;
            }

            // Стальные и сталежелезобетонные пролётные строения, металлические опоры — 1,4
            if (material.IsMetal())
            {
                return 1.4d;
            }

            // Железобетонные пролётные строения — 1,3
            if (material.IsConcrete())
            {
                return 1.3d;
            }

            return NormConstants.MinStrainCoefficient;
        }

        private static double GetDynamicCoefficientPillar(PillarMaterial material)
        {
            if (material.IsMetal())
            {
                return 1.4d;
            }

            // Железобетонные (бетонные) массивные опоры, грунтовые основания и все фундаменты — 1,0
            // Деревянные мосты / массивные — 1,0
            if (material.IsMassiveOrTimberSupport() || material.IsWood())
            {
                return 1d;
            }

            // Тонкостенные пустотелые (незаполненные) и стоечные опоры — 1,3
            if (material.IsConcrete())
            {
                return 1.3d;
            }

            return 1d;
        }
    }
}
