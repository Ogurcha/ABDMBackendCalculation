using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainSelector
    {
        IEnumerable<StrainResult> GetStrainResults(Dictionary<RoadRule, 
            (double X, VehicleStrain Strain)[]> orderedTrajectoriesMap, 
            IntervalModel intervalModel, 
            VehicleRollingBigModel data);
    }
}