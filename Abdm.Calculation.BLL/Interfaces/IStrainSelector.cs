using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainSelector
    {
        IEnumerable<StrainResultUnpopulated> GetStrainResults(
            Dictionary<RoadRule, StrainsInTrajectory[]> orderedTrajectoriesMap, 
            IntervalModel intervalModel, 
            VehicleRollingBigModel data);
    }
}