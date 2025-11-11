using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients
{
    public class DynamicMovementPillarCoefficientCalculator : AbstractCoefficientCalculator<PillarMaterial>, ICoefficientCalculator
    {
        public StrainCoefficientTypeEnum StrainCoefficientType => StrainCoefficientTypeEnum.DynamicMovement;

        public StrainCalculationGroupTypeEnum[] StrainCalculationTypes =>
        [
            StrainCalculationGroupTypeEnum.Pillar,
        ];

        public override double GetCoefficient(double lambda, LoadGroupTypeEnum loadGroupType, PillarMaterial? materialNullable)
        {
            if (materialNullable is not PillarMaterial material)
            {
                return NormConstants.MinStrainCoefficient;
            }

            switch (loadGroupType)
            {
                case LoadGroupTypeEnum.Common when IsMetal():
                case LoadGroupTypeEnum.AClass when IsMetal():
                    if (IsHanging())
                    {
                        return 1 + 50 / (70 + lambda);
                    }
                    if (!IsHanging())
                    {
                        return 1 + 15 / (37.5 + lambda);
                    }
                    break;
                case LoadGroupTypeEnum.Common:
                case LoadGroupTypeEnum.AClass:
                    return Math.Min(1.3, 1.0 + 0.0075 * (45 - lambda));
                case LoadGroupTypeEnum.Single:
                case LoadGroupTypeEnum.NClass:
                case LoadGroupTypeEnum.Track:
                    return 1.1d;
                default:
                    return NormConstants.MinStrainCoefficient;
            }

            return NormConstants.MinStrainCoefficient;

            bool IsMetal() => new MaterialTypeEnum[]
            {
                MaterialTypeEnum.Metal,
                MaterialTypeEnum.SteelReinforcedConcrete
            }.Contains(material.MaterialType);

            bool IsHanging() => new PillarTypeEnum[]
            {
                PillarTypeEnum.SuspendedCableStayedStructurePylon
            }.Contains(material.PillarType);
        }
    }
}
