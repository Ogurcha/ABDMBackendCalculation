using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISurfaceBinaryParserFactory
    {
        ISurfaceBinaryParser? GetParser(StrainCalculationTypeEnum strainCalculationType);
    }
}