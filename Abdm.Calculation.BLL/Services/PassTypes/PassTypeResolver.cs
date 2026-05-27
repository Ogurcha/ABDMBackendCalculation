using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Services.PassTypes.Conditions;

namespace Abdm.Calculation.BLL.Services.PassTypes
{
    public class PassTypeResolver(IStrainCoefficientFactory strainCoefficientFactory) : IPassTypeResolver
    {
        public List<(IPassTypeCondition condition, PassTypeEnum passType)> PassTypeConditions =
            new()
            {
                (new NoLimitCondition(), PassTypeEnum.NoLimit),
                (new WithoutPedestrianCondition(), PassTypeEnum.WithoutPedestrian),
                (new Speed10Condition(), PassTypeEnum.MaxSpeed10),
                (new SingleAutoOnlyCondition(), PassTypeEnum.SingleAutoOnly)
            };

        public StrainCalculationGroupTypeEnum[] StrainCalculationTypes =>
        [
            StrainCalculationGroupTypeEnum.Default,
            StrainCalculationGroupTypeEnum.Slab,
            StrainCalculationGroupTypeEnum.Pillar,
        ];

        public PassTypeEnum Resolve(IList<StrainResult> strainResults, VehicleRollingSmallModel data)
        {
            double? dynamicCoefficient = null;
            if (strainCoefficientFactory.GetStrainCalculator(StrainCoefficientTypeEnum.DynamicMovement, data.Surface.StrainCalculationGroupType) is ICoefficientCalculator calculator)
            {
                dynamicCoefficient = calculator.Get(data.Surface.Lambda, data.Load.Type, data.Surface.Material);
            }

            foreach (var c in PassTypeConditions)
            {
                if (c.condition.CanPassCondition(strainResults, data.Surface, dynamicCoefficient))
                {
                    return c.passType;
                }
            }

            return PassTypeEnum.Denied;
        }
    }
}
