using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainSelector
    {
        IEnumerable<StrainResultUnpopulated> SelectBestStrainResult(
            Dictionary<RoadRule, StrainsInMaximums[]> orderedTrajectoriesMap, 
            IntervalModel intervalModel, 
            VehicleRollingBigModel data);
    }
}