using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public interface IVehicleTrajectoryService
    {
        ProfileYZ? GetProfileYZ(Mesh mesh, double X);
        VehicleTrajectory[] GetVehicleTrajectories([DisallowNull] VehicleXPosition[] vehicleXPositions, Mesh mesh);

        VehicleTrajectory? GetVehicleTrajectory(VehicleXPosition xPosition, Mesh mesh);
    }
}