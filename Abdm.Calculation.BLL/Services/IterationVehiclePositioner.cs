using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Extensions;

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
    public class IterationVehiclePositioner(IVehicleTrajectoryService vehicleTrajectoryService) : IVehiclePositioner
    {
        private double CachedDelta = double.NaN;

        private double CachedDeltaBackwards = double.NaN;

        /// <summary>
        /// Найти максимальное напряжение от нагрузки в траектории в определенной позиции.
        /// </summary>
        [MemberNotNull]
        public VehicleStrain GetStrainFromVehicleInPosition(VehicleTrajectory trajectory, double startingPosition, PassTypeSmallModel data)
        {
            if (!data.Load.IsSymmetric!.Value && data.Direction == Enums.DriveDirectionEnum.Bidirection)
            {
                return MathExtensions.Max(GetStrain(true, ref CachedDelta), GetStrain(false, ref CachedDeltaBackwards));
            }
            else if (data.Direction == Enums.DriveDirectionEnum.Backward)
            {
                return GetStrain(false, ref CachedDeltaBackwards);
            }
            else
            {
                return GetStrain(true, ref CachedDelta);
            }

            VehicleStrain GetStrain(bool loadDirectionForward, ref double cachedDelta)
            {
                var goForward = loadDirectionForward;
                if (double.IsNaN(cachedDelta))
                {
                    cachedDelta = goForward ? - data.Load.Length : data.Load.Length;
                }
                var position = startingPosition + cachedDelta;
                VehicleStrain? oldStrain = null;
                var maxSteps = (int)Math.Round(data.Load.Length / NormConstants.StrainMeasuringStepSize);
                var stepSize = loadDirectionForward ? NormConstants.StrainMeasuringStepSize : -NormConstants.StrainMeasuringStepSize;

                while (maxSteps > 0)
                {
                    maxSteps--;
                    var strain = vehicleTrajectoryService.GetStrainOnTrajectory(trajectory,
                        position,
                        data.Load,
                        !loadDirectionForward);

                    if (strain.SumStrain <= oldStrain?.SumStrain)
                    {
                        if (goForward)
                        {
                            goForward = false;
                            position -= stepSize;
                            strain = oldStrain ?? strain;
                        }
                        else
                        {
                            cachedDelta = Math.Min(data.Load.Length, Math.Max(-data.Load.Length, position + stepSize - startingPosition));
                            return oldStrain;
                        }
                    }

                    if (goForward)
                    {
                        position += stepSize;
                    }
                    else
                    {
                        position -= stepSize;
                    }

                    oldStrain = strain;
                }

                return oldStrain!;
            }
        }
    }
}
