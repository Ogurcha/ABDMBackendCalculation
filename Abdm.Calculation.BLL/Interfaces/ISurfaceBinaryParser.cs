using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISurfaceBinaryParser
    {
        IList<StrainCalculationGroupTypeEnum> StrainCalculationTypes { get; }

        SurfaceDataDto ParseData(SurfaceDataDto surface, BinaryReader reader, PassageInterval[] intervals);
    }
}