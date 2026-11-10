using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.StrainCoefficients
{
    public static class MaterialHelpers
    {
        public static bool IsMetal(this IMaterial material) => ((MaterialTypeEnum[])
        [
        MaterialTypeEnum.Metal,
        MaterialTypeEnum.SteelReinforcedConcrete
        ]).Contains(material.MaterialType);

        public static bool IsConcrete(this IMaterial material) => ((MaterialTypeEnum[])
        [
            MaterialTypeEnum.ReinforcedConcrete,
            MaterialTypeEnum.PnReinforcedConcrete
        ]).Contains(material.MaterialType);

        public static bool IsHanging(this SurfaceMaterial material) => ((StaticSystemTypeEnum[])
        [
            StaticSystemTypeEnum.Hanging,
            StaticSystemTypeEnum.CableStayed
        ]).Contains(material.StaticSystemType);

        public static bool IsArched(this SurfaceMaterial material) => ((StaticSystemTypeEnum[])
        [
            StaticSystemTypeEnum.ArchedNonStrut,
            StaticSystemTypeEnum.HingedLessArch,
            StaticSystemTypeEnum.SingleHingedArch,
            StaticSystemTypeEnum.DoubleHingedArch,
            StaticSystemTypeEnum.ThreeHingedArch
        ]).Contains(material.StaticSystemType);

        public static bool IsSuperArched(this SurfaceMaterial material) => ((SuperStructureTypeEnum[])
        [
            SuperStructureTypeEnum.ThroughTrusses,
            SuperStructureTypeEnum.Arches,
            SuperStructureTypeEnum.RigidBeamArches,
            SuperStructureTypeEnum.ArchesWithAStaircase,
        ]).Contains(material.SuperStructureType);

        public static bool IsPylon(this PillarMaterial material) => ((PillarTypeEnum[])
        [
            PillarTypeEnum.SuspendedCableStayedStructurePylon
        ]).Contains(material.PillarType);

        /// <summary>
        /// Массивные опоры (бетон/камень), деревянные опоры — динамический коэффициент 1,0 по табл. А.7 ОДМ.
        /// </summary>
        public static bool IsMassiveOrTimberSupport(this PillarMaterial material) =>
            material.IsWood()
            || material.IsStone()
            || material.MaterialType is MaterialTypeEnum.Concrete or MaterialTypeEnum.RubbleConcrete;

        public static bool IsWood(this IMaterial material) => ((MaterialTypeEnum[])
        [ 
            MaterialTypeEnum.Timber,
            MaterialTypeEnum.GluedLaminatedTimber
        ]).Contains(material.MaterialType);

        public static bool IsBeam(this SurfaceMaterial material) => ((StaticSystemTypeEnum[])
        [
            StaticSystemTypeEnum.SplitBeam,
            StaticSystemTypeEnum.ContinuousBeam,
            StaticSystemTypeEnum.ThermallyContinuousBeam,
            StaticSystemTypeEnum.SingleCantileverBeam,
            StaticSystemTypeEnum.DoubleCantileverBeam
        ]).Contains(material.StaticSystemType);

        public static bool IsStone(this IMaterial material) => ((MaterialTypeEnum[])
        [
            MaterialTypeEnum.Masonry,
            MaterialTypeEnum.Brickwork
        ]).Contains(material.MaterialType);
    }
}
