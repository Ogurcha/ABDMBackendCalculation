namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Параметры использования ИССО. Чем серьезнее условия, тем тяжелее ИССО будет пройти проверку.
    /// </summary>
    public class RoadRule
    {
        /// <summary>
        /// Есть ли пешеходная полоса. Учитывать ли вес пешеходов
        /// </summary>
        public bool IsPedestrianAllowed { get; set; }

        /// <summary>
        /// Разрешено ли носиться по мосту под сотку.
        /// </summary>
        public bool IsDynamicMovement { get; set; }

        /// <summary>
        /// Наличие полосы безопасности. Проверять ли без наезда на полосу безопасности
        /// </summary>
        public bool HasSafetyLine { get; set; }

        /// <summary>
        /// Максимальное количество ТС в колонне.
        /// </summary>
        public int MaxVehicleInTrajectory { get; set; }

        /// <summary>
        /// Максимальное количество траекторий движения на одном интервале
        /// </summary>
        public int MaxTrajectoriesInInterval { get; set; }

        /// <summary>
        /// Максимальное количество траекторий движения суммарно на всех интервалах
        /// </summary>
        public required int MaxTrajectoriesTotal { get; set; }

        /// <summary>
        /// Минимальное расстояние между центров траекторий движения. 
        /// </summary>
        public double MinTrajectoryDistance { get; set; }

        /// <summary>
        /// Дополнительный отступ от края проезда/ограждения до габарита ТС (не между траекториями).
        /// Для ряда старых норм (СН 200-62 и ранее) для одиночных НК/НГ равен 0,25 м.
        /// </summary>
        public double ExtraBarrierDistance { get; set; } = 0d;

        /// <summary>
        /// Рассчитывать ли доп нагрузку от пробки.
        /// Которая считается, как равномерно распределенная доп нагрузка по всей длине профиля
        /// </summary>
        public bool DoTrafficJamLoadCalculation { get; set; }
    }
}
