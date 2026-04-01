using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Extensions;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Сервис, располагающий ТС каждой своей тележкой в точку экстремума
    /// </summary>
    public class AxleVehiclePositioner(IVehicleTrajectoryService vehicleTrajectoryService) : IVehiclePositioner
    {
        public VehicleStrain GetStrainFromVehicleInPosition(VehicleTrajectory trajectory, 
            double position, 
            VehicleRollingSmallModel data)
        {
            var directions = data.Load.ActualDirection;
            var vehicleStrains = directions.Select(GetStrain).OrderDescending().ToArray();
            var result = vehicleStrains.First();
            result.InvertedDirectionStrain = vehicleStrains.ElementAtOrDefault(1);
            return result;

            VehicleStrain GetStrain(bool loadDirectionForward)
            {
                return data.Load.Axles.Max(axle => vehicleTrajectoryService.GetStrainOnTrajectory(
                    trajectory,
                    position - axle.AbsolutePosition,
                    data.Load,
                    !loadDirectionForward))!;
            }
        }
    }
}
