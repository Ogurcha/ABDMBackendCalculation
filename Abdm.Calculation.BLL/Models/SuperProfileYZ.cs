using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.Graphics.Models
{
    /// <summary>
    /// Суперпрофиль - сумма всех профилей, полученных от каждого колеса транспортного средства
    /// Сумма взвешенная с учетом веса тележек ТС и с учётом сдвига тележек относительно друг друга
    /// </summary>
    public class SuperProfileYZ : ProfileYZ, ISuperProfileYZ
    {
        public required VehicleTrajectory VehicleTrajectoryRef { get; set; }

        public required LoadModel LoadModelRef { get; set; }
    }

    /// <summary>
    /// пара суперпрофилей для двунаправленного движения
    /// </summary>
    public class SuperProfileYZPair : ISuperProfileYZ
    {
        /// <summary>
        /// значение по оси X - индентификатор суперпрофиля
        /// </summary>
        public required double X { get; set; }

        public required VehicleTrajectory VehicleTrajectoryRef { get; set; }

        public required LoadModel LoadModelRef { get; set; }

        public required ProfileYZ ForwardMovementSuperProfile { get; set; }

        public required ProfileYZ BackwardMovementSuperProfile { get; set; }
    }
}
