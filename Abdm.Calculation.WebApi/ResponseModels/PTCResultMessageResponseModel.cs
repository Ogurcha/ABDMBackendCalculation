namespace Abdm.Calculation.WebApi.ResponseModels
{
    public class PTCResultMessageResponseModel
    {
        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        public long c_isso { get; set; }

        /// <summary>
        /// Номер чекпоинта данного сооружения
        /// </summary>
        public int n { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        public long c_nagruzka { get; set; }

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        public int direction { get; set; }

        /// <summary>
        /// номер выбранного снипа, по которому пойдут расчет
        /// </summary>
        public int snip { get; set; }

        /// <summary>
        /// Рассчитанное условие пропуска
        /// </summary>
        public int pass_type { get; set; }

        /// <summary>
        /// можно ли проезжать (рассчитывается из PassType). 1 - зеленый свет, 0 - нельзя, 
        /// </summary>
        public int? allowed { get; set; }

        /// <summary>
        /// Интервалы между нагрузками.
        /// </summary>
        public required double[] intervals { get; set; }

        /// <summary>
        /// Нагрузка тележек. не обязательна
        /// </summary>
        public string? data { get; set; }
    }
}
