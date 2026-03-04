using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainCalculator
    {
        IEnumerable<VehicleStrain?> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, VehicleRollingSmallModel data);

        Dictionary<RoadRule, (double X, VehicleStrain strain)[]> GetStrainsMap(IntervalModel intervalModel, VehicleRollingBigModel data);
        TrafficJamStrain GetTrafficJamStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data);
    }
}