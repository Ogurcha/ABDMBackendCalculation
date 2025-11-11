using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    /// <summary>
    /// Суперпрофиль - сумма всех профилей, полученных от каждого колеса транспортного средства
    /// Сумма взвешенная с учетом веса тележек ТС и с учётом сдвига тележек относительно друг друга
    /// </summary>
    public interface ISuperProfileYZ
    {
        /// <summary>
        /// значение по оси X - индентификатор профиля
        /// </summary>
        public double X { get; set; }

        public VehicleTrajectory VehicleTrajectoryRef { get; set; }

        public LoadModel LoadModelRef { get; set; }
    }
}
