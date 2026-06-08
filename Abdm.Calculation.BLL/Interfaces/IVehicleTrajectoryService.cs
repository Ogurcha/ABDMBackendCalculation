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

        VehicleTrajectory? GetVehicleTrajectoryBase(VehicleXPosition xPosition,
            VehicleRollingBigModel data);

        VehicleXPosition[] CalculateVehiclePositionsIncludingWheelOffsets(
            VehicleRollingBigModel data,
            PassageInterval passageInterval);

        VehicleStrain GetStrainOnTrajectory(VehicleTrajectory trajectory, 
            double Y, 
            LoadModel load, 
            bool invertAxles, 
            bool doSlabVersion);
    }
}