using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainCalculator
    {
        IEnumerable<double> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, PassTypeSmallModel data, bool doTrafficJamCalulation);
        Dictionary<RoadRule, (double X, double Strain)[]> GetStrainsMap(IntervalModel intervalModel, IEnumerable<RoadRule> roadRules, PassTypeSmallModel data);
    }
}