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
        public long C_isso { get; set; }

		/// <summary>
		/// Номер чекпоинта данного сооружения
		/// </summary>
		public int Number { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        required public long С_nagruzka { get; set; }

		/// <summary>
		/// номер выбранного снипа, по которому пойдут расчет
		/// </summary>
		public ais7PcSnip Snip { get; set; } = ais7PcSnip.odm16;

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        public ais7DriveDirection Direction { get; set; } = ais7DriveDirection.Bidirection;

		/// <summary>
		/// Подробные характеристики нагрузки на данное сооружение
		/// </summary>
		public Schema Load_schema { get; set; }

		public Surface Surface { get; set; }

        public Roadway Roadway { get; set; }
    }
}