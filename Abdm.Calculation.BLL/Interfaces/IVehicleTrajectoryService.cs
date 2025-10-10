using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IVehicleTrajectoryService
    {
        IntervalModel GetIntervalModel(PassTypeSmallModel data,
            Mesh mesh,
            PassageInterval interval, 
            RoadRule[] roadRules);

        ProfileYZ? GetProfileYZ(Mesh mesh, 
            double X,
            double wheelLength);

        VehicleTrajectory[] GetVehicleTrajectories([DisallowNull] VehicleXPosition[] vehicleXPositions, 
            Mesh mesh,
            Axle[] axles);

        VehicleTrajectory? GetVehicleTrajectory(VehicleXPosition xPosition, 
            Mesh mesh,
            double wheelLength);

        VehicleXPosition[] CalculateVehiclePositionsIncludingWheelOffsets(
            double[] distinctXs,
            PassageInterval passageInterval,
            LoadModel loadModel,
            RoadRule[] roadRules);
    }
}