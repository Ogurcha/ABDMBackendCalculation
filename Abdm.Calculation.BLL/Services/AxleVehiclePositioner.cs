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
        public VehicleStrain GetStrainFromVehicleInPosition(VehicleTrajectory trajectory, double position, VehicleRollingSmallModel data)
        {

            if (!data.Load.IsSymmetric!.Value && data.Direction == Enums.DriveDirectionEnum.Bidirection)
            {
                var forwardStrain = GetStrain(true);
                var backwardStrain = GetStrain(false);
                if (forwardStrain.CompareTo(backwardStrain) > 0)
                {
                    forwardStrain.InvertedDirectionStrain = backwardStrain;
                    return forwardStrain;
                }
                else
                {
                    backwardStrain.InvertedDirectionStrain = forwardStrain;
                    return backwardStrain;
                }                
            }
            else if (data.Direction == Enums.DriveDirectionEnum.Backward)
            {
                return GetStrain(false);
            }
            else
            {
                return GetStrain(true);
            }

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
