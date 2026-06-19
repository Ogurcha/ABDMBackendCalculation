using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IProfileYZService
    {
        VehicleXPosition[] CalculateRequiredTrajectoryPositions(VehicleRollingBigModel dataModel, PassageInterval passageInterval, bool doTrajectoriesUnderWheels);

        Dictionary<double, ProfileYZ> CreateProfileMap(VehicleXPosition[] xPositions, VehicleRollingBigModel dataModel);
    }
}