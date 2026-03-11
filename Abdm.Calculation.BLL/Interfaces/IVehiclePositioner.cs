using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Interfaces
{
    /// <summary>
    /// Сервис позиционирования определяет, как расположить ТС в точке экстремума. 
    /// Расположить ли центром ТС в центр эекстремума, или как либо иначе...
    /// </summary>
    public interface IVehiclePositioner
    {
        VehicleStrain GetStrainFromVehicleInPosition(VehicleTrajectory trajectory, double position, VehicleRollingSmallModel data);
    }
}
