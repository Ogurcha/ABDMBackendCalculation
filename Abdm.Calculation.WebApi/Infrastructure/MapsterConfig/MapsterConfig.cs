using System;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Mapster;

namespace Abdm.Calculation.WebApi.Infrastructure.MapsterConfig
{
    public static class MapsterConfig
    {
        public static void MapsterSetup()
        {
            PTCModelFromDtoConfig();
            PTCModelToDtoConfig();
        }

        public static void PTCModelFromDtoConfig()
        {
            TypeAdapterConfig<SurfaceDataItemRequestModel, SurfacePoint>
            .NewConfig()
            .Map(dst => dst.X, src => src.X)
            .Map(dst => dst.Y, src => src.Y)
            .Map(dst => dst.Z, src => src.Z);

            TypeAdapterConfig<AxleRequestModel, AxleModel>
            .NewConfig()
            .Map(dst => dst.Y, src => src.Y)
            .Map(dst => dst.Wx, src => src.Wx)
            .Map(dst => dst.Wy, src => src.Wy)
            .Map(dst => dst.Weight, src => src.Weight)
            .Map(dst => dst.AbsolutY, src => src.AbsolutY)
            .Map(dst => dst.Wheels, src => src.Wheels);

            TypeAdapterConfig<LoadSchemaRequestModel, LoadSchema>
            .NewConfig()
            .Map(dst => dst.Id, src => src.Id)
            .Map(dst => dst.Type, src => src.Type)
            .Map(dst => dst.TypeName, src => src.TypeName)
            .Map(dst => dst.NameShort, src => src.NameShort)
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
            .Map(dst => dst.SurfacePoints, src => src.SurfacePoints)
            .Map(dst => dst.PillarData, src => src.PillarData)
            .Map(dst => dst.MaxX, src => src.MaxX)
            .Map(dst => dst.MaxY, src => src.MaxY)
            .Map(dst => dst.MaxZ, src => src.MaxZ)
            .Map(dst => dst.MinX, src => src.MinX)
            .Map(dst => dst.MinY, src => src.MinY)
            .Map(dst => dst.CheckPointType, src => src.CheckPointType)
            .Map(dst => dst.MyStrength, src => src.MyStrength)
            .Map(dst => dst.ConstLoad, src => src.ConstLoad)
            .Map(dst => dst.PedestrianLoad, src => src.PedestrianLoad)
            .Map(dst => dst.OtherLoad, src => src.OtherLoad)
            .Map(dst => dst.KStrength, src => src.KStrength)
            .AfterMapping(dst =>
            {
                if (!Enum.IsDefined(dst.CheckPointType))
                {
                    dst.CheckPointType = CheckPointEnum.None;
                }
            });

            TypeAdapterConfig<RoadwayRequestModel, Roadway>
            .NewConfig()
            .Map(dst => dst.LineNumber, src => src.LineNumber)
            .Map(dst => dst.RoadHeight, src => src.RoadHeight)
            .Map(dst => dst.LeftSafeline, src => src.LeftSafeline)
            .Map(dst => dst.RightSafeline, src => src.RightSafeline)
            .Map(dst => dst.PositionShift, src => src.PositionShift);

            TypeAdapterConfig<PassTypeCalculationRequest, PassTypeCalculationParameters>
            .NewConfig()
            .Map(dst => dst.IssoId, src => src.IssoId)
            .Map(dst => dst.CheckPointNumber, src => src.CheckPointNumber)
            .Map(dst => dst.LoadId, src => src.LoadId)
            .Map(dst => dst.Snip, src => src.Snip)
            .Map(dst => dst.Direction, src => src.Direction)
            .Map(dst => dst.LoadSchema, src => src.LoadSchema)
            .Map(dst => dst.Surface, src => src.Surface)
            .Map(dst => dst.Roadway, src => src.Roadway);
        }


        public static void PTCModelToDtoConfig()
        {
            TypeAdapterConfig<PassTypeCalculationResult, PassTypeCalculationResponse>
            .NewConfig()
            .Map(dst => dst.IssoId, src => src.IssoId)
            .Map(dst => dst.CPNumber, src => src.CPNumber)
            .Map(dst => dst.LoadId, src => src.LoadId)
            .Map(dst => dst.Direction, src => src.Direction)
            .Map(dst => dst.Snip, src => src.Snip)
            .Map(dst => dst.PassType, src => src.PassType)
            .Map(dst => dst.Allowed, src => src.Allowed)
            .Map(dst => dst.Intervals, src => src.Intervals);
        }

    }
}
