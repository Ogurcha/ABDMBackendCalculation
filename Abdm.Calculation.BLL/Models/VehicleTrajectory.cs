using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Траектория движения транспортного средства
    /// </summary>
    public class VehicleTrajectory
    {
        /// <summary>
        /// траектория колёс слева
        /// </summary>
        public ProfileYZ[]? Left { get; set; }

        /// <summary>
        /// траектория условного центра
        /// </summary>
        public required ProfileYZ Center { get; set; }

        /// <summary>
        /// траектория колёс справа
        /// </summary>
        public ProfileYZ[]? Right { get; set; }

        public double X => Center.X;
    }
}
