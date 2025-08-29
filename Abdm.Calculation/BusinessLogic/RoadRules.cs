namespace Abdm.Calculation.BusinessLogic
{
    /// <summary>
    /// Параметры использования ИССО. Чем серьезнее условия, тем тяжелее ИССО будет пройти проверку.
    /// </summary>
    public struct RoadRules
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
        /// Наличие линии безопасности. Уменьшает количество ТС, помещающихся на ИССО.
        /// </summary>
        public bool HasSafetyLine { get; set; }

        /// <summary>
        /// Максимальное количество ТС в колонне.
        /// </summary>
        public int MaxAutoInColumn { get; set; }

        /// <summary>
        /// Максимальное количество колонн.
        /// </summary>
        public int MaxColumnCount { get; set; }

        /// <summary>
        /// Минимальное расстояние между колоннами.
        /// </summary>
        public double MinColumnDistance { get; set; }

    }
}
