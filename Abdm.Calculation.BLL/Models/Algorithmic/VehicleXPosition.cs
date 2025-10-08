using System.Diagnostics.CodeAnalysis;

namespace Abdm.Calculation.BLL.Models.Algorithmic
{
    /// <summary>
    /// Абсолютное положение ТС по оси X
    /// </summary>
    public class VehicleXPosition
    {
        public VehicleXPosition (double centerXPosition, [DisallowNull] double[] halfWheelOffsets)
        {
            CenterXPosition = centerXPosition;
            var left = new List<double> ();
            var right = new List<double> ();
            foreach (var halfWheelOffset in halfWheelOffsets)
            {
                left.Add(centerXPosition - halfWheelOffset);
                right.Add(centerXPosition + halfWheelOffset);
            }
            LeftXPosition = left.ToArray();
            RightXPosition = right.ToArray();
        }

        /// <summary>
        /// Абсолютное положение колёс слева транспортного средства по оси Х
        /// </summary>
        public double[] LeftXPosition { get; set; }

        /// <summary>
        /// Абсолютное центра транспортного средства по оси Х
        /// </summary>
        public double CenterXPosition { get; set; }

        /// <summary>
        /// Абсолютное положение колёс справа транспортного средства по оси Х
        /// </summary>
        public double[] RightXPosition { get; set; }
    }
}
