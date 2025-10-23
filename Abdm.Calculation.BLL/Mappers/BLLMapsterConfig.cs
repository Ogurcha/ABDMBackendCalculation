using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.DAL.DataTransferObjects;
using Abdm.Calculation.DAL.Entities;
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
            .Map(dst => dst.Axles, src => src.Axles);

            TypeAdapterConfig<PassTypeCalculationParameters, PassTypeSmallModel>
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
                     dst.StrainCalculationType = DAL.Enums.StrainCalculationTypeEnum.Other;
                 }
                 if (!Enum.IsDefined(dst.CheckPointType))
                 {
                     dst.CheckPointType = DAL.Enums.CheckPointTypeEnum.TypNk_PS;
                 }
             });
        }
    }
}
