using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
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
            .Map(dst => dst.TotalWidth, src => src.BGabarit)
            .Map(dst => dst.AbsolutePositionLeft, src => src.BOgrazhdenieLeft)
            .Map(dst => dst.AbsolutePositionRight, src => src.BOgrazhdenieRight)
            .Map(dst => dst.SafetyLineLeft, src => src.BLp)
            .Map(dst => dst.SafetyLineRight, src => src.BPb)
            .Map(dst => dst.LaneCount, src => src.KolichestvoPolos >= 1 ? src.KolichestvoPolos : 1)
            .Map(dst => dst.Type, src => src.ProezdType);

            TypeAdapterConfig<Surface, SurfaceModel>
            .NewConfig()
            .Map(dst => dst.MaxY, src => src.MaxY)
            .Map(dst => dst.MinY, src => src.MinY)
            .Map(dst => dst.MyStrength, src => src.MyStrength)
            .Map(dst => dst.ConstLoad, src => src.ConstLoad)
            .Map(dst => dst.PedestrianLoad, src => src.PedestrianLoad)
            .Map(dst => dst.OtherLoad, src => src.OtherLoad);

            TypeAdapterConfig<PassTypeCalculationParameters, LoadModel>
            .NewConfig()
            .Map(dst => dst.Direction, src => src.Direction)
            .Map(dst => dst.Width, src => src.LoadSchema.Width ?? NormConstants.DefaultVehicleWidth)
            .Map(dst => dst.Length, src => src.LoadSchema.Length ?? NormConstants.DefaultVehicleLength)
            .Map(dst => dst.Distance, src => src.LoadSchema.Distance ?? NormConstants.DefaultVehicleDistance)
            .Map(dst => dst.Axles, src => src.LoadSchema.Axles);

            TypeAdapterConfig<PassTypeCalculationParameters, PassTypeSmallModel>
            .NewConfig()
            .Map(dst => dst.Surface, src => src.Surface)
            .Map(dst => dst.Load, src => src);

        }
    }
}
