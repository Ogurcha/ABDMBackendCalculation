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
        /// Давление колеса на поверхность
        /// </summary>
        public decimal Pressure { get; set; }

        public decimal PositionX { get; set; }

        public decimal PositionY { get; set; }

        public decimal Strain { get; set; }

        public decimal Z { get; set; }

        /// <summary>
        /// Размер отпечатка колеса с учётом добавочной длины дорожной одежды (первый множитель)
        /// </summary>
        public decimal FootPrintSizeFirst { get; set; }

        /// <summary>
        /// Размер отпечатка колеса с учётом добавочной длины дорожной одежды (второй множитель)
        /// </summary>
        public decimal FootPrintSizeSecond { get; set; }

        /// <summary>
        /// Объем поверхности под отпечатком
        /// </summary>
        public decimal ZVolume { get; set; }
    }
}
