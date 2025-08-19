namespace Abdm.Calculation.Models
{
    public class CCResultMessage
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
        required public long С_nagruzka { get; set; }

        /// <summary>
		/// номер выбранного снипа, по которому пойдут расчет
		/// </summary>
		public ais7PcSnip Snip { get; set; } = ais7PcSnip.odm16;

        /// <summary>
        /// Рассчитанное условие пропуска
        /// </summary>
        public ais7PassTypeEnum PassType { get; set; }

        /// <summary>
		/// можно ли проезжать (рассчитывается из PassType). 1 - зеленый свет
		/// </summary>
		public int Allowed { get; set; }

        //не обязательно
        public float[] Intervals { get; set; }

        /// <summary>
        /// Нагрузка тележек. не обязательна
        /// </summary>
        public string Data { get; set; } //"[{\"x\": 4.64, \"y\": 0.25, \"z\": 0.091, \"load\": 0.517}, {\"x\": 7.79, \"y\": -4.45, \"z\": 1.608, \"load\": 9.277}]"
    }
}
