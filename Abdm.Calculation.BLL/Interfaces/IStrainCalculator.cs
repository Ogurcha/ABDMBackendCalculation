using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainCalculator
    {
        IEnumerable<VehicleStrain?> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, PassTypeSmallModel data, bool doTrafficJamCalulation);
        Dictionary<RoadRule, (double X, VehicleStrain Strain)[]> GetStrainsMap(IntervalModel intervalModel, IEnumerable<RoadRule> roadRules, PassTypeSmallModel data);
    }
}