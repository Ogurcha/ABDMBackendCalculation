namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Информация о колёсах
    /// </summary>
    public class Axle
    {
        public double Y { get; set; }

        public double Wx { get; set; }

        public double Wy { get; set; }

        /// <summary>
        /// Вес колеса
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Абсолютная длина проекции, с учетом текущего колеса и колёс позади
        /// </summary>
        public double AbsY { get; set; }

        public double[]? Wheels { get; set; }
    }
}
