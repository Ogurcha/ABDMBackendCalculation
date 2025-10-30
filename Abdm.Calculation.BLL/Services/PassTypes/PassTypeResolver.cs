using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Services.PassTypes
{
    public class PassTypeResolver : IPassTypeResolver
    {
        public List<(IPassTypeCondition condition, PassTypeEnum passType)> PassTypeConditions =
            new()
            {
                (new NoLimitCondition(), PassTypeEnum.NoLimit),
                (new WithoutPedestrianCondition(), PassTypeEnum.WithoutPedestrian),
                (new Speed10Condition(), PassTypeEnum.MaxSpeed10),
                (new SingleAutoOnlyCondition(), PassTypeEnum.SingleAutoOnly)
            };

        public StrainCalculationTypeEnum[] StrainCalculationTypes =>
        [
            StrainCalculationTypeEnum.st10,
            StrainCalculationTypeEnum.st12,
            StrainCalculationTypeEnum.st14,
            StrainCalculationTypeEnum.st20,
            StrainCalculationTypeEnum.st22,
            StrainCalculationTypeEnum.st24,
            StrainCalculationTypeEnum.st30,
            StrainCalculationTypeEnum.st50,
            StrainCalculationTypeEnum.st60,
            StrainCalculationTypeEnum.st70,
            StrainCalculationTypeEnum.st80,
            StrainCalculationTypeEnum.st90,
            StrainCalculationTypeEnum.st510,
            StrainCalculationTypeEnum.st520,
            StrainCalculationTypeEnum.st530,
            StrainCalculationTypeEnum.st553,
            StrainCalculationTypeEnum.st556,
            StrainCalculationTypeEnum.st558,
            StrainCalculationTypeEnum.st540,
            StrainCalculationTypeEnum.st560,
            StrainCalculationTypeEnum.st610,
            StrainCalculationTypeEnum.st630,
            StrainCalculationTypeEnum.st632,
            StrainCalculationTypeEnum.st710,
            StrainCalculationTypeEnum.st720,
            StrainCalculationTypeEnum.st730,
            StrainCalculationTypeEnum.st740,
            StrainCalculationTypeEnum.st760,
            StrainCalculationTypeEnum.st770,
            StrainCalculationTypeEnum.st790,
        ];

        public PassTypeEnum Resolve(List<StrainResult> strainResults, SurfaceModel surfaceModel)
        {
            foreach (var c in PassTypeConditions)
            {
                if (c.condition.CanPassCondition(strainResults, surfaceModel))
                {
                    return c.passType;
                }
            }

            return PassTypeEnum.Denied;
        }
    }
}
