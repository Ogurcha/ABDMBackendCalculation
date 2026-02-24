using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.DAL.DataTransferObjects;
using Abdm.Calculation.DAL.Entities;
using Abdm.Calculation.DAL.Enums;
using Abdm.Calculation.SteelConcrete.Models;
using Mapster;

namespace Abdm.Calculation.BLL.Mappers
{
    public static class BLLMapsterConfig
    {
        public static void BLLMapsterSetup()
        {
            TypeAdapterConfig<PassageIntervalDto, PassageInterval>
            .NewConfig()
            .Map(dst => dst.TotalWidth, src => src.b_gab)
            .Map(dst => dst.AbsolutePositionLeft, src => src.b_ogr_l)
            .Map(dst => dst.AbsolutePositionRight, src => src.b_ogr_r)
            .Map(dst => dst.SafetyLineLeft, src => src.b_lp)
            .Map(dst => dst.SafetyLineRight, src => src.b_pb)
            .Map(dst => dst.LaneCount, src => src.k_polos >= 1 ? src.k_polos : 1)
            .Map(dst => dst.Type, src => src.w_proezd);

            TypeAdapterConfig<Surface, SurfaceModel>
            .NewConfig()
            .Map(dst => dst.MaxY, src => src.MaxY)
            .Map(dst => dst.MinY, src => src.MinY)
            .Map(dst => dst.MyStrength, src => src.MyStrength)
            .Map(dst => dst.ConstLoad, src => src.ConstLoad)
            .Map(dst => dst.PedestrianLoad, src => src.PedestrianLoad)
            .Map(dst => dst.OtherLoad, src => src.OtherLoad);

            TypeAdapterConfig<LoadSchema, LoadModel>
            .NewConfig()
            .Map(dst => dst.Width, src => src.Width ?? NormConstants.DefaultVehicleWidth)
            .Map(dst => dst.Length, src => src.Length ?? NormConstants.DefaultVehicleLength)
            .Map(dst => dst.Distance, src => src.Distance ?? NormConstants.DefaultVehicleDistance)
            .Map(dst => dst.Axles, src => src.Axles)
            .Map(dst => dst.Type, src => src.Type);

            TypeAdapterConfig<PassTypeCalculationParameters, VehicleRollingSmallModel>
            .NewConfig()
            .Map(dst => dst.Direction, src => src.Direction)
            .Map(dst => dst.Surface, src => src.Surface)
            .Map(dst => dst.Load, src => src.LoadSchema);

            TypeAdapterConfig<SurfaceRawDataDto, SurfaceDataDto>
            .NewConfig()
            .Map(dst => dst.StrainCalculationType, src => src.c_cptype)
            .Map(dst => dst.CheckPointType, src => src.c_typnk)
            .Map(dst => dst.Lambda, src => src.lambda)
            .AfterMapping(dst =>
             {
                 if (!Enum.IsDefined(dst.StrainCalculationType))
                 {
                     dst.StrainCalculationType = StrainCalculationTypeEnum.Other;
                 }
                 if (!Enum.IsDefined(dst.CheckPointType))
                 {
                     dst.CheckPointType = CheckPointTypeEnum.Surface;
                 }
             });

            TypeAdapterConfig<PillarMaterialDto, PillarMaterial>
            .NewConfig()
            .Map(dst => dst.MaterialType, src => src.c_matop)
            .Map(dst => dst.PillarType, src => src.c_typop)
            .AfterMapping(dst =>
            {
                if (!Enum.IsDefined(dst.MaterialType))
                {
                    dst.MaterialType = MaterialTypeEnum.Other;
                }
                if (!Enum.IsDefined(dst.PillarType))
                {
                    dst.PillarType = PillarTypeEnum.Other;
                }
            });

            TypeAdapterConfig<SurfaceMaterialDto, SurfaceMaterial>
            .NewConfig()
            .Map(dst => dst.MaterialType, src => src.c_mpsbm)
            .Map(dst => dst.StaticSystemType, src => src.c_typps)
            .Map(dst => dst.SuperStructureType, src => src.c_sistps)
            .AfterMapping(dst =>
            {
                if (!Enum.IsDefined(dst.MaterialType))
                {
                    dst.MaterialType = MaterialTypeEnum.Other;
                }
                if (!Enum.IsDefined(dst.StaticSystemType))
                {
                    dst.StaticSystemType = StaticSystemTypeEnum.Other;
                }
                if (!Enum.IsDefined(dst.StaticSystemType))
                {
                    dst.SuperStructureType = SuperStructureTypeEnum.Other;
                }
            });

            TypeAdapterConfig<Surface, IssoSteelConcreteParameters>
            .NewConfig()
            .Map(dst => dst.WorkSign, src => src.WorkSign)
            .Map(dst => dst.Es, src => src.Es)
            .Map(dst => dst.Ea, src => src.Ea)
            .Map(dst => dst.Eb, src => src.Eb)
            .Map(dst => dst.TetaKrParam, src => src.TetaKr)
            .Map(dst => dst.EpsilonBetaLim, src => src.EpsilonBetaLim)
            .Map(dst => dst.Rs1, src => src.Rs1)
            .Map(dst => dst.Rs2, src => src.Rs2)
            .Map(dst => dst.Rr, src => src.Rr)
            .Map(dst => dst.Rb, src => src.Rb)
            .Map(dst => dst.TMax, src => src.Tmax)
            .Map(dst => dst.PlateType, src => src.PlateType)
            .Map(dst => dst.L, src => src.L)
            .Map(dst => dst.Sd, src => src.Sd)
            .Map(dst => dst.SigmaBetaKrParam, src => src.SigmaBetaKr)
            .Map(dst => dst.SigmaAlfaKrParam, src => src.SigmaAlfaKr)
            .Map(dst => dst.SigmaBetaShrParam, src => src.SigmaBetaShr)
            .Map(dst => dst.SigmaAlfaShrParam, src => src.SigmaAlfaShr)
            .Map(dst => dst.SigmaBetaTParam, src => src.SigmaBetaT)
            .Map(dst => dst.SigmaAlfaTParam, src => src.SigmaAlfaT)
            .Map(dst => dst.M1, src => src.M1)
            .Map(dst => dst.M2g, src => src.M2g)
            .Map(dst => dst.X1Coefficient, src => src.Xsi1)
            .Map(dst => dst.Mp, src => src.Mp);
        }

        public static StrainCalculationGroupTypeEnum Map(this StrainCalculationTypeEnum source)
        {
            return source switch
            {
                StrainCalculationTypeEnum.st10 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st12 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st14 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st20 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st22 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st24 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st30 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st50 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st60 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st80 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st90 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st510=> StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st520 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st530 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st553 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st556 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st558 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st540 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st560 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st610 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st630 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st632 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st710 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st720 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st730 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st740 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st760 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st770 => StrainCalculationGroupTypeEnum.Default,
                StrainCalculationTypeEnum.st790 => StrainCalculationGroupTypeEnum.Default,

                StrainCalculationTypeEnum.st40 => StrainCalculationGroupTypeEnum.SteelConcrete,
                StrainCalculationTypeEnum.st70 => StrainCalculationGroupTypeEnum.Pillar,
                _ => StrainCalculationGroupTypeEnum.Unknown
            };
        }
    }
}
