using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients.snip1984
{
    public class ABCoefficientProvider : CoefficientProvider
    {
        public ABCoefficientProvider()
        {
            WorksInSnips = [
                SnipEnum.sn84
            ];
            WorksForLoads = [
                LoadGroupTypeEnum.AB,
            ];
        }

        public override double GetBasicCoefficent(double lambda) =>
            1.2d;

        public override double GetDynamicCoefficient(double lambda, IMaterial material, StrainCalculationGroupTypeEnum calculationType)
        {
            if (material.IsConcrete() && calculationType 
                != StrainCalculationGroupTypeEnum.Pillar 
                && ((SurfaceMaterial)material).IsBeam())
            {
                return 1 + (81 - lambda) / 135;
            }
            if (material.IsMetal())
            {
                return 1 + (81 - lambda) / 115;
            }
            if (material.IsWood())
            {
                return 1.1d;
            }

            return 1.2d;
        }
    }
}
