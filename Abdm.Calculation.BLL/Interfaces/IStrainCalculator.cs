using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainCalculator
    {
        IEnumerable<VehicleStrain?> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, VehicleRollingSmallModel data);

        Dictionary<RoadRule, StrainsInMaximums[]> GenerateStrainsMap(IntervalModel intervalModel, VehicleRollingBigModel data);

        TrafficJamStrain GetTrafficJamStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data);
        bool TryGetStrainForEachPositivePiece(VehicleTrajectory trajectory, VehicleRollingSmallModel data, out IEnumerable<VehicleStrain> vehicleStrains);
    }
}