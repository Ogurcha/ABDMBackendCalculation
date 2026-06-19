namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Абсолютное положение ТС по оси X. Прекурсор объекта <see cref="VehicleTrajectory"/>
    /// </summary>
    public class VehicleXPosition
    {
        /// <summary>
        /// Абсолютное положение колёс слева транспортного средства по оси Х. 
        /// Ключ - <see cref="Axle.WheelsDistance"/>
        /// </summary>
        public required Dictionary<double, double> LeftXPosition { get; set; }

        /// <summary>
        /// Абсолютное центра транспортного средства по оси Х
        /// </summary>
        public required double CenterXPosition { get; set; }

        /// <summary>
        /// Абсолютное положение колёс справа транспортного средства по оси Х. 
        /// Ключ - <see cref="Axle.WheelsDistance"/>
        /// </summary>
        public required Dictionary<double, double> RightXPosition { get; set; }
    }
}
