using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainSelector
    {
        IEnumerable<StrainResult> GetStrainResults(Dictionary<RoadRule, 
            (double X, VehicleStrain Strain)[]> orderedTrajectoriesMap, 
            IntervalModel intervalModel, 
            IEnumerable<RoadRule> roadRules, 
            PassTypeSmallModel data,
            Mesh mesh);
    }
}