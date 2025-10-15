using System.Text.Json.Serialization;

namespace Abdm.Calculation.DAL.Entities
{
    public class PassageIntervalDto
    {
        /// <summary>
        /// Общая ширина интервала
        /// </summary>
        [JsonPropertyName("b_gab")]
        public double b_gab { get; set; }

        /// <summary>
        /// Ограждение слева
        /// </summary>
        [JsonPropertyName("b_ogr_l")]
        public double? b_ogr_l { get; set; }

        /// <summary>
        /// Ограждение справа
        /// </summary>
        [JsonPropertyName("b_ogr_r")]
        public double? b_ogr_r { get; set; }

        /// <summary>
        /// Полоса безопасности слева
        /// </summary>
        [JsonPropertyName("b_lp")]
        public double? b_lp { get; set; }

        /// <summary>
        /// Полоса безопасности справа
        /// </summary>
        [JsonPropertyName("b_pb")]
        public double? b_pb { get; set; }

        /// <summary>
        /// Количество полос движения на данном интервале
        /// </summary>
        [JsonPropertyName("k_polos")]
        public int k_polos { get; set; }

        /// <summary>
        /// Тип движения на интервале
        /// </summary>
        [JsonPropertyName("w_proezd")]
        public int w_proezd { get; set; }
    }
}
