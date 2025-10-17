using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ITrajectorySelector
    {
        IEnumerable<double> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, LoadModel load, bool doTrafficJamCalulation);
        Dictionary<RoadRule, (double X, double Strain)[]> GetTrajectoriesStrainsMap(IntervalModel intervalModel, IEnumerable<RoadRule> roadRules, PassTypeSmallModel data);
    }
}