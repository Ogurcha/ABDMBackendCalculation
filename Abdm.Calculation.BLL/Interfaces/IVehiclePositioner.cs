using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IVehiclePositioner
    {
        double GetStrainFromVehicleInPosition(VehicleTrajectory trajectory, double position, LoadModel load);
        double GetStrainFromVehicleInPositionNoCaching(VehicleTrajectory trajectory, double startingPosition, LoadModel load);
    }
}