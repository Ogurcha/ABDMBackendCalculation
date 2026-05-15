using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainResultPopulator
    {
        List<StrainResult> PopulateStrainResults(IList<StrainResultUnpopulated> list, VehicleRollingSmallModel data);
    }
}