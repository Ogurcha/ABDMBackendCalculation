using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Models.Parameters
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
        /// Наличие линии безопасности. Уменьшает количество ТС, помещающихся на ИССО.
        /// </summary>
        public bool HasSafetyLine { get; set; }

        /// <summary>
        /// Максимальное количество ТС в колонне.
        /// </summary>
        public int MaxAutoInColumn { get; set; }

        /// <summary>
        /// Максимальное количество колонн по нормам.
        /// Оверрайдится, если иссо физически вмещает меньше колонн
        /// </summary>
        public int MaxColumnCount { get; set; }

        /// <summary>
        /// Минимальное расстояние между колоннами.
        /// </summary>
        public double MinColumnDistance { get; set; }

        /// <summary>
        /// Рассчитывать ли доп нагрузку от пробки.
        /// Которая считается, как равномерно распределенная доп нагрузка по всей длине профиля
        /// </summary>
        public bool DoTrafficJamLoadCalulation { get; set; }
    }
}
