using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Сервис, располагающий ТС каждой своей тележкой в точку экстремума
    /// </summary>
    public class AxleVehiclePositioner : IVehiclePositioner
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
                data.Load.Axles
                .Select(a => a.Position)
                .Append(data.Load.Length / 2)
                .Max(relativePosition => data.VehicleStrainProvider!.GetStrainOnTrajectory(
                    trajectory,
                    position - relativePosition,
                    data.Load,
                    !loadDirectionForward))!;

            VehicleStrain GetStrainSlab(bool loadDirectionForward) =>
                data.Load.Axles
                .Select(a => a.Position)
                .Append(data.Load.Length / 2)
                .Max(relativePosition => data.VehicleStrainProvider!.GetStrainOnTrajectory(
                    trajectory,
                    position - relativePosition,
                    data.Load,
                    !loadDirectionForward))!;
        }
    }
}
