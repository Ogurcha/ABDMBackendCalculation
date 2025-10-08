namespace Abdm.Calculation.BLL.Helpers
{
    /// <summary>
    /// Статические данные, описанные в каких либо нормах
    /// </summary>
    public static class NormConstants
    {

        /// <summary>
        /// Шаг, с которым надо чекать позиционирование ТС в экстремуме профиля на предмет макс напряжения
        /// </summary>
        public const double StrainMeasuringStepSize = 0.1d;

        /// <summary>
        /// Нормативный отступ от центра ТС до края интервала движения ВНЕ ЗАВИСИМОСТИ от реальной ширины ТС
        /// </summary>
        public const double VehicleEdgeDistance = 1.5d;


        public const double slExtraDistance = 0.25d;
    }
}
