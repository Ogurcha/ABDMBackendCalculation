using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Models.StrainAnalysis
{
    public class AnalysisWheel
    {
        /// <summary>
        /// Вес колеса
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Ширина отпечатка колеса
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Длина отпечатка колеса
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Давление колеса на поверхность
        /// </summary>
        public double Pressure { get; set; }

        public double PositionX { get; set; }

        public double PositionY { get; set; }

        public double Strain { get; set; }

        public double Z { get; set; }
    }
}
