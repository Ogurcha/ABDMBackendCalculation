using System;
using System.Linq;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;

namespace Abdm.Calculation.WebApi.Mappers
{
    /// <summary>
    /// Простой маппер для калькулятора расчета условий пропуска
    /// </summary>
    public class PassTypeModelsMapper : IPassTypeModelsMapper
    {
        public PTCRequestMessage FromDTO(PTCRequestMessageRequestModel dto)
        {
            var ladingSchema = dto.load_schema ?? new LadingSchemaRequestModel();
            var axles = ladingSchema.axles ?? [];
            var surface = dto.surface ?? new SurfaceRequestModel();
            var surfacePoints = surface.surface_data ?? [];
            var pillarData = surface.line_data ?? [];
            var roadway = dto.roadway ?? new RoadwayRequestModel();
            return new PTCRequestMessage
            {
                IssoId = dto.c_isso,
                CPNumber = dto.number,
                LadingId = dto.c_nagruzka,
                Snip = (SnipEnum)dto.snip,
                Direction = (DriveDirectionEnum)dto.direction,
                LadingSchema = new LadingSchema
                {
                    Id = Enum.IsDefined(typeof(LadingEnum), ladingSchema.id)
                    ? (LadingEnum)ladingSchema.id
                    : LadingEnum.User,
                    Type = Enum.TryParse(ladingSchema.type_id, out LadingGroupTypeEnum ladingGroupTypeEnum)
                    ? ladingGroupTypeEnum
                    : LadingGroupTypeEnum.Common,
                    TypeName = ladingSchema.type ?? string.Empty,
                    NameShort = ladingSchema.name ?? string.Empty,
                    Width = ladingSchema.width,
                    Length = ladingSchema.length,
                    Distance = ladingSchema.distance,
                    Axles = [.. axles.Select(a => new AxleModel
                    {
                        Y = a.y,
                        Wx = a.wx,
                        Wy = a.wy,
                        Weight = a.weight,
                        AbsolutY = a.absY,
                        Wheels = a.wheels,
                    })]
                },
                Surface = new Surface
                {
                    SurfacePoints = [.. surfacePoints.Select(v => (v.x, v.y, v.z))],
                    PillarData = pillarData,
                    MaxX = surface.maxX,
                    MaxY = surface.maxY,
                    MaxZ = surface.maxZ,
                    MinX = surface.minX,
                    MinY = surface.minY,
                    CheckPointType = Enum.IsDefined(typeof(CheckPointEnum), surface.cpVid)
                    ? (CheckPointEnum)surface.cpVid
                    : CheckPointEnum.None,
                    MyStrength = surface.myStrength,
                    ConstLoad = surface.constLoad,
                    PedestrianLoad = surface.constPesh,
                    OtherLoad = surface.constOther,
                    KStrength = surface.kStrength,
                },
                Roadway = new Roadway
                {
                    LineNumber = roadway.line_number,
                    RoadHeight = roadway.road_height,
                    LeftSafeline = roadway.left_safeline,
                    RightSafeline = roadway.right_safeline,
                    PositionShift = roadway.position_shift
                }
            };
        }

        public PTCResultMessageResponseModel ToDTO(PTCResultMessage model)
        {
            return new PTCResultMessageResponseModel
            {
                c_isso = model.IssoId,
                n = model.CPNumber,
                c_nagruzka = model.LadingId,
                direction = (int)model.Direction,
                snip = (int)model.Snip,
                pass_type = (int)model.PassType,
                allowed = (int)model.Allowed,
                intervals = model.Intervals
            };
        }
    }
}
