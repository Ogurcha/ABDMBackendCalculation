using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.SurfaceData.Parsers
{
    public class PillarSurfaceBinaryParser(IPillarDataService pillarDataService) : BaseSurfaceBinaryParser, ISurfaceBinaryParser
    {
        public override IList<StrainCalculationGroupTypeEnum> StrainCalculationTypes =>
        [
            StrainCalculationGroupTypeEnum.Pillar
        ];

        public override SurfaceDataDto ParseData(SurfaceDataDto surface, BinaryReader reader, PassageInterval[] intervals)
        {
            SkipSomeBytes(reader);
            surface.PointsCount = reader.ReadInt32();
            surface.Points = ReadPointsYZ(reader, surface.PointsCount).ToArray();

            pillarDataService.UpdateSurfaceDataFromPillarData(surface, intervals);

            return surface;
        }
    }
}
