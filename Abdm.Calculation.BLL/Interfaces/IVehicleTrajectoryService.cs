using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IVehicleTrajectoryService
    {
        IntervalModel GetIntervalModel(
            VehicleRollingBigModel data,
            PassageInterval interval);

        ProfileYZ? GetProfileYZ(Mesh mesh, 
            double X);

        VehicleTrajectory[] GetVehicleTrajectories([DisallowNull] VehicleXPosition[] vehicleXPositions, 
            Mesh mesh);

        VehicleTrajectory? GetVehicleTrajectoryBase(VehicleXPosition xPosition, 
            Mesh mesh);

        VehicleTrajectory? GetVehicleTrajectory(Mesh mesh, 
            LoadModel loadModel, 
            double centerXPosition);

        VehicleXPosition[] CalculateVehiclePositionsIncludingWheelOffsets(
            double[] distinctXs,
            PassageInterval passageInterval,
            LoadModel loadModel,
            RoadRule[] roadRules);

        VehicleStrain GetStrainOnTrajectory(VehicleTrajectory trajectory, double Y, LoadModel load, bool invertAxles);
    }
}