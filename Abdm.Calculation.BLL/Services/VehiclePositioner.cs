using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Класс для прокатки ТС по траектории 
    /// Дело в том, что мы заранее не знаем точку с максимальным напряжением 
    /// Нельзя просто взять ТС и поставить в максимальную точку своей серединой 
    /// так как ТС может быть тяжелее сзади. 
    /// Посчитать идеальную позицию мы тоже не можем, так как мы не знаем характеристи поверхности влияния заранее 
    /// Бывают поверхности влияния пологие с одной стороны и отвесно уходящие вверх с другой 
    /// И в таком случае нам надо расположить ТС только с одной стороны от максимума 
    /// Чтобы хоть как то оптимизировать процесс, я кэширую удачную дельту, чтобы в следующем цикле считать уже от удачной позиции
    /// </summary>
    public class VehiclePositioner(IVehicleTrajectoryService vehicleTrajectoryService) : IVehiclePositioner
    {
        public double? CachedDelta { get; set; }

        /// <summary>
        /// Найти максимальное напряжение от нагрузки в траектории в определенной позиции.
        /// </summary>
        public double GetStrainFromVehicleInPosition(VehicleTrajectory trajectory, double startingPosition, LoadModel load)
        {
            if (CachedDelta == null)
            {
                CachedDelta = -load.Length;
            }
            var position = startingPosition + CachedDelta.Value;
            double? oldStrain = null;
            var maxSteps = (int)Math.Round(load.Length / NormConstants.StrainMeasuringStepSize);
            var goForward = true;

            while (maxSteps > 0) {
                maxSteps--;
                var strain = vehicleTrajectoryService.GetStrainOnTrajectory(trajectory,
                    position,
                    load);

                if (strain <= oldStrain)
                {
                    if (goForward) {
                        goForward = false;
                        position -= NormConstants.StrainMeasuringStepSize;
                        strain = oldStrain ?? strain;
                    }
                    else
                    {
                        CachedDelta = Math.Min(load.Length, Math.Max(-load.Length, position - startingPosition));
                        return oldStrain.Value;
                    }
                }

                if (goForward)
                {
                    position += NormConstants.StrainMeasuringStepSize;
                }
                else
                {
                    position -= NormConstants.StrainMeasuringStepSize;
                }

                oldStrain = strain;
            }

            return oldStrain ?? 0;
        }

        public double GetStrainFromVehicleInPositionNoCaching(VehicleTrajectory trajectory, double startingPosition, LoadModel load)
        {
            return vehicleTrajectoryService.GetStrainOnTrajectory(trajectory, startingPosition, load);
        }
    }
}
