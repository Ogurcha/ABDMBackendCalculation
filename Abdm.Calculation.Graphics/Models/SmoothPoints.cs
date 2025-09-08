using g4;

namespace Abdm.Calculation.Graphics.Models
{
    /// <summary>
    /// Список гладких (плавных) точек кривой аппроксимации поверхности влияни.
    /// </summary>
    public class SmoothPoints
    {
        /// <summary>
        /// точки, через которые проходит кривая
        /// </summary>
        public required Vector3d[] Points { get; set; }
    }
}
