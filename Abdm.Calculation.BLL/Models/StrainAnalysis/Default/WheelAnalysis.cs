namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class WheelAnalysis
    {
        public int Number { get; set; }

        public int SubNumber { get; set; }

        /// <summary>
        /// Вес колеса
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Ширина отпечатка колеса
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// Длина отпечатка колеса
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// Давление колеса на поверхность
        /// </summary>
        public decimal Pressure { get; set; }

        public decimal PositionX { get; set; }

        public decimal PositionY { get; set; }

        public decimal Strain { get; set; }

        public decimal Z { get; set; }
    }
}
