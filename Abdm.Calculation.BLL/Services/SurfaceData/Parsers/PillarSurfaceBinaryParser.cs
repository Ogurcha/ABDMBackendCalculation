using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Services.SurfaceData.Parsers
{
    public class PillarSurfaceBinaryParser(IPillarDataService pillarDataService) : BaseSurfaceBinaryParser, ISurfaceBinaryParser
    {
        public override IList<StrainCalculationTypeEnum> StrainCalculationTypes =>
            new List<StrainCalculationTypeEnum> {
                StrainCalculationTypeEnum.st70 
            };

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
