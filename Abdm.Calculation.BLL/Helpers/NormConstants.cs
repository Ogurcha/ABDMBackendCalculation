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

        /// <summary>
        /// Дефолтная Ширина ТС, в большинстве норм именно такая ширина
        /// </summary>
        public const double DefaultVehicleWidth = 3d;

        /// <summary>
        /// Дефолтная Длина ТС, в большинстве норм именно такая длина
        /// </summary>
        public const double DefaultVehicleLength = 4.5d;

        /// <summary>
        /// Дефолтная Расстояние между ТС, в большинстве норм именно такое расстояние
        /// </summary>
        public const double DefaultVehicleDistance = 3d;

        /// <summary>
        /// Дефолтное расстояние между осями. Для дефолтной выбрал Н11 (НК-80)
        /// </summary>
        public static double[] DefaultAxleDistance { get; internal set; } = [ 2.7d ];

        /// <summary>
        /// Равномерно расположенные ТС по всей длине ИССО считаются, как равномерная 5% нагрузка по всей длине ИССО
        /// </summary>
        public const double TrafficJamApproximationParam = 0.05d;
    }
}
