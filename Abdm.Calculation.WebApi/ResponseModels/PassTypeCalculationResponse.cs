using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels
{
    public class PassTypeCalculationResponse
    {
        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        [JsonPropertyName("c_isso")]
        public long IssoId { get; set; }

        /// <summary>
        /// Номер чекпоинта данного сооружения
        /// </summary>
        [JsonPropertyName("n")]
        public int CPNumber { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        [JsonPropertyName("c_nagruzka")]
        public long LoadId { get; set; }

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        [JsonPropertyName("direction")]
        public int Direction { get; set; }

        /// <summary>
        /// номер выбранного снипа, по которому пойдут расчет
        /// </summary>
        [JsonPropertyName("snip")]
        public int Snip { get; set; }

        /// <summary>
        /// Рассчитанное условие пропуска
        /// </summary>
        [JsonPropertyName("pass_type")]
        public int PassType { get; set; }

        /// <summary>
        /// можно ли проезжать (рассчитывается из PassType). 1 - зеленый свет, 0 - нельзя, 
        /// </summary>
        [JsonPropertyName("allowed")]
        public int Allowed { get; set; }

        /// <summary>
        /// Интервалы между нагрузками.
        /// </summary>
        [JsonPropertyName("intervals")]
        public required double[] Intervals { get; set; }

        /// <summary>
        /// Нагрузка тележек. не обязательна
        /// </summary>
        [JsonPropertyName("data")]
        public string? Data { get; set; }
    }
}
