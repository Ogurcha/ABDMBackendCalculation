using System;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Mapster;

namespace Abdm.Calculation.WebApi.Mappers
{
    public static class MapsterConfig
    {
        public static void MapsterSetup()
        {
            BLLMapsterConfig.BLLMapsterSetup();
            PTCModelFromDtoConfig();
            PTCModelToDtoConfig();
        }

        public static void PTCModelFromDtoConfig()
        {
            TypeAdapterConfig<SurfaceDataItemRequestModel, SurfacePoint>
            .NewConfig()
            .Map(dst => dst.X, src => src.x)
            .Map(dst => dst.Y, src => src.y)
            .Map(dst => dst.Z, src => src.z);

            TypeAdapterConfig<AxleRequestModel, AxleModel>
            .NewConfig()
            .Map(dst => dst.Y, src => src.y)
            .Map(dst => dst.Wx, src => src.wx)
            .Map(dst => dst.Wy, src => src.wy)
            .Map(dst => dst.Weight, src => src.weight)
            .Map(dst => dst.AbsolutY, src => src.absY)
            .Map(dst => dst.Wheels, src => src.wheels);

            TypeAdapterConfig<LadingSchemaRequestModel, LadingSchema>
            .NewConfig()
            .Map(dst => dst.Id, src => src.id)
            .Map(dst => dst.Type, src => src.type_id)
            .Map(dst => dst.TypeName, src => src.type)
            .Map(dst => dst.NameShort, src => src.name)
            .Map(dst => dst.Width, src => src.width)
            .Map(dst => dst.Length, src => src.length)
            .Map(dst => dst.Distance, src => src.distance)
            .Map(dst => dst.Axles, src => src.axles)
            .AfterMapping(dst =>
                {
                    if (!Enum.IsDefined(dst.Id))
                    {
                        dst.Id = LadingEnum.User;
                    }
                    if (!Enum.IsDefined(dst.Type))
                    {
                        dst.Type = LadingGroupTypeEnum.Common;
                    }
                }
            );

            TypeAdapterConfig<SurfaceRequestModel, Surface>
            .NewConfig()
            .Map(dst => dst.SurfacePoints, src => src.surface_data)
            .Map(dst => dst.PillarData, src => src.line_data)
            .Map(dst => dst.MaxX, src => src.maxX)
            .Map(dst => dst.MaxY, src => src.maxY)
            .Map(dst => dst.MaxZ, src => src.maxZ)
            .Map(dst => dst.MinX, src => src.minX)
            .Map(dst => dst.MinY, src => src.minY)
            .Map(dst => dst.CheckPointType, src => src.cpVid)
            .Map(dst => dst.MyStrength, src => src.myStrength)
            .Map(dst => dst.ConstLoad, src => src.constLoad)
            .Map(dst => dst.PedestrianLoad, src => src.constPesh)
            .Map(dst => dst.OtherLoad, src => src.constOther)
            .Map(dst => dst.KStrength, src => src.kStrength)
            .AfterMapping(dst =>
            {
                if (!Enum.IsDefined(dst.CheckPointType))
                {
                    dst.CheckPointType = CheckPointEnum.None;
                }

            });

            TypeAdapterConfig<RoadwayRequestModel, Roadway>
            .NewConfig()
            .Map(dst => dst.LineNumber, src => src.line_number)
            .Map(dst => dst.RoadHeight, src => src.road_height)
            .Map(dst => dst.LeftSafeline, src => src.left_safeline)
            .Map(dst => dst.RightSafeline, src => src.right_safeline)
            .Map(dst => dst.PositionShift, src => src.position_shift);

            TypeAdapterConfig<PassTypeCalculationRequest, PassTypeCalculationParameters>
            .NewConfig()
            .Map(dst => dst.IssoId, src => src.c_isso)
            .Map(dst => dst.CPNumber, src => src.number)
            .Map(dst => dst.LadingId, src => src.c_nagruzka)
            .Map(dst => dst.Snip, src => src.snip)
            .Map(dst => dst.Direction, src => src.direction)
            .Map(dst => dst.LadingSchema, src => src.load_schema)
            .Map(dst => dst.Surface, src => src.surface)
            .Map(dst => dst.Roadway, src => src.roadway);
        }


        public static void PTCModelToDtoConfig()
        {
            TypeAdapterConfig<PassTypeCalculationResult, PassTypeCalculationResponse>
            .NewConfig()
            .Map(dst => dst.c_isso, src => src.IssoId)
            .Map(dst => dst.n, src => src.CPNumber)
            .Map(dst => dst.c_nagruzka, src => src.LadingId)
            .Map(dst => dst.direction, src => src.Direction)
            .Map(dst => dst.snip, src => src.Snip)
            .Map(dst => dst.pass_type, src => src.PassType)
            .Map(dst => dst.allowed, src => default(int?), srcCond => srcCond.Allowed == AllowedEnum.Undefined)
            .Map(dst => dst.allowed, src => src.Allowed)
            .Map(dst => dst.intervals, src => src.Intervals);
        }

    }
}
