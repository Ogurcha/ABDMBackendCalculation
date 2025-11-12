using System;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.Maths.Models;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Mapster;

namespace Abdm.Calculation.WebApi.Infrastructure.MapsterConfig
{
    public static class MapsterConfig
    {
        public static void MapsterSetup()
        {
            TypeAdapterConfig<SurfaceDataItemRequestModel, Vector3D>
            .NewConfig()
            .Map(dst => dst.X, src => src.X)
            .Map(dst => dst.Y, src => src.Y)
            .Map(dst => dst.Z, src => src.Z);

            TypeAdapterConfig<AxleRequestModel, Axle>
            .NewConfig()
            .Map(dst => dst.RelativePosition, src => src.Y)
            .Map(dst => dst.Wx, src => src.Wx)
            .Map(dst => dst.Wy, src => src.Wy)
            .Map(dst => dst.Weight, src => src.Weight)
            .Map(dst => dst.AbsolutePosition, src => src.AbsY)
            .Map(dst => dst.WheelsDistance, src => src.Wheels);

            TypeAdapterConfig<LoadSchemaRequestModel, LoadSchema>
            .NewConfig()
            .Map(dst => dst.Id, src => src.Id)
            .Map(dst => dst.Type, src => src.TypeId)
            .Map(dst => dst.TypeName, src => src.Type)
            .Map(dst => dst.NameShort, src => src.Name)
            .Map(dst => dst.Width, src => src.Width)
            .Map(dst => dst.Length, src => src.Length)
            .Map(dst => dst.Distance, src => src.Distance)
            .Map(dst => dst.Axles, src => src.Axles)
            .AfterMapping(dst =>
            {
                if (!Enum.IsDefined(dst.Id))
                {
                    dst.Id = LoadEnum.User;
                }
                if (!Enum.IsDefined(dst.Type))
                {
                    dst.Type = LoadGroupTypeEnum.Common;
                }
            }
            );

            TypeAdapterConfig<SurfaceRequestModel, Surface>
            .NewConfig()
            .Map(dst => dst.SurfacePoints, src => src.SurfaceData)
            .Map(dst => dst.PillarData, src => src.LineData)
            .Map(dst => dst.MaxX, src => src.MaxX)
            .Map(dst => dst.MaxY, src => src.MaxY)
            .Map(dst => dst.MaxZ, src => src.MaxZ)
            .Map(dst => dst.MinX, src => src.MinX)
            .Map(dst => dst.MinY, src => src.MinY)
            .Map(dst => dst.MyStrength, src => src.MyStrength == default(int) ? src.SuperStrength : src.MyStrength)
            .Map(dst => dst.ConstLoad, src => src.ConstLoad)
            .Map(dst => dst.PedestrianLoad, src => src.ConstPesh)
            .Map(dst => dst.OtherLoad, src => src.ConstOther)
            .Map(dst => dst.KStrength, src => src.KStrength);

            TypeAdapterConfig<RoadwayRequestModel, Roadway>
            .NewConfig()
            .Map(dst => dst.LineNumber, src => src.LineNumber ?? 1)
            .Map(dst => dst.RoadHeight, src => src.RoadHeight)
            .Map(dst => dst.LeftSafeline, src => src.LeftSafeline)
            .Map(dst => dst.RightSafeline, src => src.RightSafeline)
            .Map(dst => dst.PositionShift, src => src.PositionShift);

            TypeAdapterConfig<PassTypeCalculationRequest, PassTypeCalculationParameters>
            .NewConfig()
            .Map(dst => dst.IssoId, src => src.CIsso)
            .Map(dst => dst.CheckPointNumber, src => src.Number)
            .Map(dst => dst.LoadId, src => src.CNagruzka)
            .Map(dst => dst.Snip, src => src.Snip)
            .Map(dst => dst.Direction, src => src.Direction)
            .Map(dst => dst.LoadSchema, src => src.LoadSchema)
            .Map(dst => dst.Surface, src => src.Surface)
            .Map(dst => dst.Roadway, src => src.Roadway)
            .Map(dst => dst.SecondaryLoadSchema, src => src.SecondaryLoadSchema);


            TypeAdapterConfig<PassTypeCalculationResult, PassTypeCalculationResponse>
            .NewConfig()
            .Map(dst => dst.CIsso, src => src.IssoId)
            .Map(dst => dst.N, src => src.CPNumber)
            .Map(dst => dst.CNagruzka, src => src.LoadId)
            .Map(dst => dst.Direction, src => src.Direction)
            .Map(dst => dst.Snip, src => src.Snip)
            .Map(dst => dst.PassType, src => src.PassType)
            .Map(dst => dst.Allowed, src => src.Allowed)
            .Map(dst => dst.Intervals, src => src.Intervals);
        }
    }
}
