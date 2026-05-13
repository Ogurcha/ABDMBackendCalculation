using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainResultService
    {
        StrainResult[] GetStrainResults(VehicleRollingBigModel dataModel, IEnumerable<IntervalModel> intervals);
    }
}