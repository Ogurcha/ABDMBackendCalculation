namespace Abdm.Calculation.SteelConcrete.Enums
{
    public enum SteelConcretePassCheckResultEnum
    {
        /// <summary>
        /// Можно проезжать
        /// </summary>
        CanPass,

        /// <summary>
        /// Можно проезжать только без пешеходов
        /// </summary>
        CanPassWithoutPedestrianOnly,

        /// <summary>
        /// Невозможно применить этот метод расчета
        /// </summary>
        CannotUseSteelConcreteCheck
    }
}
