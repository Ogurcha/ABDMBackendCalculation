using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainSelector
    {
        IList<StrainResultUnpopulated> SelectBestStrainResult(
            StrainMap[] strainMaps,
            VehicleRollingBigModel data);
    }
}