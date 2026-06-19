using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.LowLevelCalculation
{
    public class VehicleTrajectoryManager : IVehicleTrajectoryManager
    {
        public virtual IntervalModel GetIntervalModel(
            VehicleRollingBigModel dataModel,
            PassageInterval interval,
            bool doTrajectoriesUnderWheels,
            IProfileYZService profileYZService)
        {
            var result = new IntervalModel() { PassageIntervalRef = interval };
            var distinctXs = profileYZService.CalculateRequiredTrajectoryPositions(
                dataModel,
                interval,
                doTrajectoriesUnderWheels);

            var profileMap = profileYZService.CreateProfileMap(distinctXs, dataModel);

            result.Trajectories = distinctXs
                .Select(x => GetVehicleTrajectory(x, profileMap, dataModel, profileYZService))
                .OfType<VehicleTrajectory>()
                .ToArray();

            return result;
        }

        public virtual VehicleTrajectory? GetVehicleTrajectory(
            VehicleXPosition xPosition,
            Dictionary<double, ProfileYZ> profileMap,
            VehicleRollingBigModel dataModel,
            IProfileYZService profileYZService)
        {
            var left = new Dictionary<double, ProfileYZ>();
            foreach (var item in xPosition.LeftXPosition)
            {
                profileMap.TryGetValue(item.Value, out ProfileYZ? profileYZ);
                if (profileYZ == null)
                {
                    return null;
                }
                left.Add(item.Key, profileYZ);
            }
            var right = new Dictionary<double, ProfileYZ>();
            foreach (var item in xPosition.RightXPosition)
            {
                profileMap.TryGetValue(item.Value, out ProfileYZ? profileYZ);
                if (profileYZ == null)
                {
                    return null;
                }
                right.Add(item.Key, profileYZ);
            }
            profileMap.TryGetValue(xPosition.CenterXPosition, out ProfileYZ? center);
            if (center == null)
            {
                return null;
            }

            return new VehicleTrajectory
            {
                Center = center,
                Left = new SortedList<double, ProfileYZ>(left),
                Right = new SortedList<double, ProfileYZ>(right),
            };
        }
    }
}
