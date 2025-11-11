using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Сервис, располагающий ТС каждой своей тележкой в точку экстремума
    /// </summary>
    public class AxleVehiclePositioner(IVehicleTrajectoryService vehicleTrajectoryService) : IVehiclePositioner
    {
        public double GetStrainFromVehicleInPosition(VehicleTrajectory trajectory, double position, PassTypeSmallModel data)
        {
            if (!data.Load.IsSymmetric!.Value && data.Direction == Enums.DriveDirectionEnum.Bidirection)
            {
                return Math.Max(GetStrain(true), GetStrain(false));
            }
            else if (data.Direction == Enums.DriveDirectionEnum.Backward)
            {
                return GetStrain(false);
            }
            else
            {
                return GetStrain(true);
            }

            double GetStrain(bool loadDirectionForward)
            {
                return data.Load.Axles.Max(axle => vehicleTrajectoryService.GetStrainOnTrajectory(
                    trajectory,
                    position - axle.AbsolutePosition,
                    data.Load,
                    !loadDirectionForward));
            }
        }
    }
}
