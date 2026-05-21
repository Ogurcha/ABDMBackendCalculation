using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients
{
    public class DynamicMovementCoefficientCalculator : AbstractCoefficientCalculator<SurfaceMaterial>, ICoefficientCalculator
    {
        public StrainCoefficientTypeEnum StrainCoefficientType => StrainCoefficientTypeEnum.DynamicMovement;

        public StrainCalculationGroupTypeEnum[] StrainCalculationTypes =>
        [
            StrainCalculationGroupTypeEnum.Default,
            StrainCalculationGroupTypeEnum.SteelConcrete,
            StrainCalculationGroupTypeEnum.Slab,
        ];

        public override double GetCoefficient(double lambda, LoadGroupTypeEnum loadGroupType, SurfaceMaterial? materialNullable)
        {
            if (materialNullable is not SurfaceMaterial material)
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
                case LoadGroupTypeEnum.Common when IsConcrete():
                case LoadGroupTypeEnum.AClass when IsConcrete():
                    if (!IsArched())
                    {
                        return 1 + (45 - lambda) / 135;
                    }
                    if (IsArched() && IsSuperArched())
                    {
                        return 1 + (70 - lambda) / 250;
                    }
                    break;
                case LoadGroupTypeEnum.Single:
                case LoadGroupTypeEnum.NClass:
                case LoadGroupTypeEnum.Track:
                    return 1.1d;
                case LoadGroupTypeEnum.AB when IsConcrete():
                    if (IsBeam())
                    {
                        return 1 + (81 - lambda) / 135;
                    }
                    break;
                case LoadGroupTypeEnum.AB when IsMetal():
                    return 1 + (81 - lambda) / 115;
                case LoadGroupTypeEnum.AB when IsWood():
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

            bool IsConcrete() => new MaterialTypeEnum[]
            {
                MaterialTypeEnum.ReinforcedConcrete,
                MaterialTypeEnum.PnReinforcedConcrete
            }.Contains(material.MaterialType);

            bool IsWood() => new MaterialTypeEnum[]
            {
                MaterialTypeEnum.Timber,
                MaterialTypeEnum.GluedLaminatedTimber
            }.Contains(material.MaterialType);

            bool IsHanging() => new StaticSystemTypeEnum[]
            {
                StaticSystemTypeEnum.Hanging,
                StaticSystemTypeEnum.CableStayed
            }.Contains(material.StaticSystemType);

            bool IsArched() => new StaticSystemTypeEnum[]
            {
                StaticSystemTypeEnum.ArchedNonStrut,
                StaticSystemTypeEnum.HingedLessArch,
                StaticSystemTypeEnum.SingleHingedArch,
                StaticSystemTypeEnum.DoubleHingedArch,
                StaticSystemTypeEnum.ThreeHingedArch
            }.Contains(material.StaticSystemType);

            bool IsBeam() => new StaticSystemTypeEnum[]
            {
                StaticSystemTypeEnum.SplitBeam,
                StaticSystemTypeEnum.ContinuousBeam,
                StaticSystemTypeEnum.ThermallyContinuousBeam,
                StaticSystemTypeEnum.SingleCantileverBeam,
                StaticSystemTypeEnum.DoubleCantileverBeam
            }.Contains(material.StaticSystemType);

            bool IsSuperArched() => new SuperStructureTypeEnum[]
            {
                SuperStructureTypeEnum.ThroughTrusses,
                SuperStructureTypeEnum.Arches,
                SuperStructureTypeEnum.RigidBeamArches,
                SuperStructureTypeEnum.ArchesWithAStaircase,
            }.Contains(material.SuperStructureType);
        }
    }
}
