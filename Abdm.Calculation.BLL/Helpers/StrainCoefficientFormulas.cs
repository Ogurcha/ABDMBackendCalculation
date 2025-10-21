namespace Abdm.Calculation.BLL.Helpers
{
    public static class StrainCoefficientFormulas
    {
        /// <summary>
        /// Рассчет динамического коеффициента для норм ОДМ 218.4.025-2016
        /// </summary>
        public static double GetDynamicMovementCoefficient(double lambda)
        {
            return Math.Max(1, Math.Min(1.3, 1 + (45 - lambda) / 135));
        }

        /// <summary>
        /// Рассчет статического коеффициента для норм ОДМ 218.4.025-2016
        /// </summary>
        public static double GetBasicStrainCoefficient(double lambda)
        {
            return Math.Max(1, Math.Min(1.4, lambda >= 30 ? 1.2 : 1.2 + 0.01 * (30 - lambda)));
        }

        /// <summary>
        /// Рассчет динамического коеффициента для норм ОДМ 218.4.025-2016
        /// </summary>
        public static double GetTrafficJamStrainCoefficient(double lambda)
        {
            return 1.2d;
        }
    }
}