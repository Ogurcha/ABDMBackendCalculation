using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// реквест сообщение для начала расчётов
    /// </summary>
    public class PassTypeCalculationParameters
    {
        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        public long IssoId { get; set; }

        /// <summary>
        /// Номер чекпоинта данного сооружения
        /// </summary>
        public int CPNumber { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        public int LadingId { get; set; }

        /// <summary>
        /// номер выбранного снипа, по которому пойдут расчет
        /// </summary>
        public SnipEnum Snip { get; set; } = SnipEnum.odm16;

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        public DriveDirectionEnum Direction { get; set; } = DriveDirectionEnum.Bidirection;

        /// <summary>
        /// Подробные характеристики нагрузки на данное сооружение
        /// </summary>
        public required LadingSchema LadingSchema { get; set; }

        /// <summary>
        /// Характеристики "поверхности влияния" иссо
        /// </summary>
        public required Surface Surface { get; set; }

        /// <summary>
        /// Характеристики пути
        /// </summary>
        public required Roadway Roadway { get; set; }
    }
}
