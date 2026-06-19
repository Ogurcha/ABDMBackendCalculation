using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IVehicleTrajectoryManager
    {
        IntervalModel GetIntervalModel(VehicleRollingBigModel dataModel, 
            PassageInterval interval, 
            bool doTrajectoriesUnderWheels);

        VehicleTrajectory? GetVehicleTrajectory(VehicleXPosition xPosition, 
            Dictionary<double, ProfileYZ> profileMap, 
            VehicleRollingBigModel dataModel);
    }
}