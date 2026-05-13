using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainResultPopulator
    {
        StrainResult PopulateStrainResult(StrainResultUnpopulated unpopulated, VehicleRollingSmallModel data);
    }
}