using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISurfaceBinaryParser
    {
        IList<StrainCalculationTypeEnum> StrainCalculationTypes { get; }

        SurfaceDataDto ParseData(SurfaceDataDto surface, BinaryReader reader, PassageInterval[] intervals);
    }
}