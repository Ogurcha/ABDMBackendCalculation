using System.Drawing;
using System.Numerics;

namespace Abdm.Calculation.Models
{
    /// <summary>
    /// реквест сообщение для начала расчётов
    /// </summary>
    public class CCRequestMessage
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
        public long NagruzkaId { get; set; }

		/// <summary>
		/// номер выбранного снипа, по которому пойдут расчет
		/// </summary>
		public SnipEnum Snip { get; set; } = SnipEnum.odm16;

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        public DriveDirection Direction { get; set; } = DriveDirection.Bidirection;

		/// <summary>
		/// Подробные характеристики нагрузки на данное сооружение
		/// </summary>
		public NagruzkaSchema NagruzkaSchema { get; set; }

        /// <summary>
        /// Характеристики поверхности сооружения
        /// </summary>
		public Surface Surface { get; set; }

        /// <summary>
        /// Характеристики пути
        /// </summary>
        public Roadway Roadway { get; set; }
    }
}