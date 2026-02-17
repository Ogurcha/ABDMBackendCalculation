using System.Diagnostics.CodeAnalysis;
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
        [MemberNotNull]
        public VehicleStrain GetStrainFromVehicleInPosition(VehicleTrajectory trajectory, double position, PassTypeSmallModel data)
        {

            if (!data.Load.IsSymmetric!.Value && data.Direction == Enums.DriveDirectionEnum.Bidirection)
            {
                return MathExtensions.Max(GetStrain(true), GetStrain(false));
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
