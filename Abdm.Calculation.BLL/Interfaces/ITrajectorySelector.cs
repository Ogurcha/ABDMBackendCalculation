using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ITrajectorySelector
    {
        Dictionary<RoadRule, (double X, double Strain)[]> GetTrajectoriesStrainsMap(IntervalModel intervalModel, IEnumerable<RoadRule> roadRules, PassTypeSmallModel data);
    }
}