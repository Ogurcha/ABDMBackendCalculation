using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Models.Algorithmic;
using Abdm.Calculation.BLL.Models.Parameters;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IVehicleTrajectoryService
    {
        ProfileYZ? GetProfileYZ(Mesh mesh, 
            double X,
            double wheelLength);

        VehicleTrajectory[] GetVehicleTrajectories([DisallowNull] VehicleXPosition[] vehicleXPositions, 
            Mesh mesh,
            Axle[] axles);

        VehicleTrajectory? GetVehicleTrajectory(VehicleXPosition xPosition, 
            Mesh mesh,
            double wheelLength);
    }
}