using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Services.LowLevelCalculation;

public class VehicleTrajectoryManagerVolumetric(
    IProfileYZServiceVolumetric profileYZService
    ) : VehicleTrajectoryManager(profileYZService), IVehicleTrajectoryManager
{
    public override VehicleTrajectory? GetVehicleTrajectory(VehicleXPosition xPosition, Dictionary<double, ProfileYZ> profileMap, VehicleRollingBigModel dataModel)
    {
        var baseTrajectory = base.GetVehicleTrajectory(xPosition, profileMap, dataModel);

        if (baseTrajectory == null)
        {
            return null;
        }

        foreach (var distance in dataModel.Data.Load.WheelOffsetsMap!.Keys)
        {
            var axles = dataModel.Data.Load.Axles.Where(a => a.WheelsDistance.Contains(distance));

            var leftProfile = baseTrajectory.Left[distance];
            var leftVolumetricProfile = profileYZService.GetProfileYZVolumetric(dataModel.Mesh, leftProfile, axles, dataModel.Data.Surface.RoadCoatSize, profileMap);
            if (leftVolumetricProfile == null)
            {
                return null;
            }
            baseTrajectory.Left[distance] = leftVolumetricProfile;

            var rightProfile = baseTrajectory.Right[distance];
            var rightVolumetricProfile = profileYZService.GetProfileYZVolumetric(dataModel.Mesh, rightProfile, axles, dataModel.Data.Surface.RoadCoatSize, profileMap);
            if (rightVolumetricProfile == null)
            {
                return null;
            }
            baseTrajectory.Right[distance] = rightVolumetricProfile;
        }

        return baseTrajectory;
    }
}