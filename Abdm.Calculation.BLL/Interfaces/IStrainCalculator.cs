using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainCalculator
    {
        IEnumerable<StrainResult> GetStrainResultFromTrajectories(Dictionary<RoadRule, 
            (double X, double Strain)[]> orderedTrajectoriesMap, 
            IntervalModel intervalModel, 
            IEnumerable<RoadRule> roadRules, 
            PassTypeSmallModel data,
            Mesh mesh);
    }
}