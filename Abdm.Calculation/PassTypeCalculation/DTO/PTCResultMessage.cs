using Abdm.Calculation.Models;

namespace Abdm.Calculation.PassTypeCalculation.DTO
{
    public class PTCResultMessage
    {
        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        public long C_isso { get; set; }

        /// <summary>
		/// Номер чекпоинта данного сооружения
		/// </summary>
		public int N { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        public long С_nagruzka { get; set; }

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        public DriveDirection Direction { get; set; } = DriveDirection.Bidirection;

        /// <summary>
		/// номер выбранного снипа, по которому пойдут расчет
		/// </summary>
		public SnipEnum Snip { get; set; } = SnipEnum.odm16;

        /// <summary>
        /// Рассчитанное условие пропуска
        /// </summary>
        public PassTypeEnum PassType { get; set; }

        /// <summary>
		/// можно ли проезжать (рассчитывается из PassType). 1 - зеленый свет, 0 - нельзя, 
		/// </summary>
		public AllowedEnum Allowed { get; set; }

        /// <summary>
        /// Интервалы между нагрузками.
        /// </summary>
        public float[] Intervals { get; set; }

        /// <summary>
        /// Нагрузка тележек. не обязательна
        /// </summary>
        public string Data { get; set; } 
    }
}
