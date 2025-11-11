using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    /// <summary>
    /// Сервис позиционирования определяет, как расположить ТС в точке экстремума. 
    /// Расположить ли центром ТС в центр эекстремума, или как либо иначе...
    /// </summary>
    public interface IVehiclePositioner
    {
        double GetStrainFromVehicleInPosition(VehicleTrajectory trajectory, double position, PassTypeSmallModel data);
    }
}
