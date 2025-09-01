using g4;

namespace Abdm.Calculation.Graphics
{
    /// <summary>
    /// Список гладких (плавных) точек кривой аппроксимации поверхности влияни.
    /// </summary>
    public class SmoothPoints
    {
        /// <summary>
        /// точки, через которые проходит кривая
        /// </summary>
        public Vector3d[] Points { get; set; }

        /// <summary>
        /// угол, под которым находится точка слева
        /// </summary>
        public double[] AngleAtLeft { get; set; }

        /// <summary>
        /// угол, под которым находится точка справа
        /// </summary>
        public double[] AngleAtRight { get; set; }
    }
}
