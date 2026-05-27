using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

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
            VehicleStrain[] vehicleStrains;
            if (data.Surface.StrainCalculationGroupType == Enums.StrainCalculationGroupTypeEnum.Slab)
            {
                vehicleStrains = directions.Select(GetStrainSlab).OrderDescending().ToArray();
            }
            else
            {
                vehicleStrains = directions.Select(GetStrain).OrderDescending().ToArray();
            }
            var result = vehicleStrains.First();
            result.InvertedDirectionStrain = vehicleStrains.ElementAtOrDefault(1);
            return result;

            VehicleStrain GetStrain(bool loadDirectionForward) =>
                data.Load.Axles.Max(axle => vehicleTrajectoryService.GetStrainOnTrajectory(
                    trajectory,
                    position - axle.AbsolutePosition,
                    data.Load,
                    !loadDirectionForward, false))!;
            
            VehicleStrain GetStrainSlab(bool loadDirectionForward) =>
                data.Load.Axles.Max(axle => vehicleTrajectoryService.GetStrainOnTrajectory(
                    trajectory,
                    position - axle.AbsolutePosition,
                    data.Load,
                    !loadDirectionForward,
                    true))!;
            
        }
    }
}
