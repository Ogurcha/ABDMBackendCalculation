using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.DAL.Enums;
using Mapster;

namespace Abdm.Calculation.BLL.Services.SurfaceData
{
    public class SurfaceBinaryParserFactory(IList<ISurfaceBinaryParser> parsers) : ISurfaceBinaryParserFactory
    {
        public ISurfaceBinaryParser? GetParser(StrainCalculationTypeEnum strainCalculationType)
            => parsers.LastOrDefault(s => s.StrainCalculationTypes.Contains(strainCalculationType.Map()));
    }
}
