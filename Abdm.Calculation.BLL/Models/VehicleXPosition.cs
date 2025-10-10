using System.Diagnostics.CodeAnalysis;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Абсолютное положение ТС по оси X
    /// </summary>
    public class VehicleXPosition
    {
        public VehicleXPosition (double centerXPosition, [DisallowNull] IEnumerable<double> halfWheelOffsets)
        {
            CenterXPosition = centerXPosition;
            var left = new Dictionary<double, double>();
            var right = new Dictionary<double, double>();
            foreach (var halfWheelOffset in halfWheelOffsets)
            {
                left.Add(halfWheelOffset * 2, centerXPosition - halfWheelOffset);
                right.Add(halfWheelOffset * 2, centerXPosition + halfWheelOffset);
            }
            LeftXPosition = left;
            RightXPosition = right;
        }

        /// <summary>
        /// Абсолютное положение колёс слева транспортного средства по оси Х. 
        /// Ключ - <see cref="Axle.WheelsDistance"/>
        /// </summary>
        public Dictionary<double, double> LeftXPosition { get; set; }

        /// <summary>
        /// Абсолютное центра транспортного средства по оси Х
        /// </summary>
        public double CenterXPosition { get; set; }

        /// <summary>
        /// Абсолютное положение колёс справа транспортного средства по оси Х. 
        /// Ключ - <see cref="Axle.WheelsDistance"/>
        /// </summary>
        public Dictionary<double, double> RightXPosition { get; set; }
    }
}
