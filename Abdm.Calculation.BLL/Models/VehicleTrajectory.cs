using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Траектория движения транспортного средства
    /// </summary>
    public class VehicleTrajectory
    {
        /// <summary>
        /// Траектория колёс слева. 
        /// Ключ - <see cref="Axle.WheelsDistance"/>
        /// </summary>
        public required Dictionary<double, ProfileYZ> Left { get; set; }

        /// <summary>
        /// траектория условного центра
        /// </summary>
        public required ProfileYZ Center { get; set; }

        /// <summary>
        /// Траектория колёс слева. 
        /// Ключ - <see cref="Axle.WheelsDistance"/>
        /// </summary>
        public required Dictionary<double, ProfileYZ> Right { get; set; }

        public double X => Center.X;
    }
}
