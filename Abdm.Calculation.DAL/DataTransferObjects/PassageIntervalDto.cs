using System.Text.Json.Serialization;

namespace Abdm.Calculation.DAL.Entities
{
    public class PassageIntervalDto
    {
        /// <summary>
        /// Общая ширина интервала
        /// </summary>
        [JsonPropertyName("b_gab")]
        public double BGabarit { get; set; }

        /// <summary>
        /// Ограждение слева
        /// </summary>
        [JsonPropertyName("b_ogr_l")]
        public double? BOgrazhdenieLeft { get; set; }

        /// <summary>
        /// Ограждение справа
        /// </summary>
        [JsonPropertyName("b_ogr_r")]
        public double? BOgrazhdenieRight { get; set; }

        /// <summary>
        /// Полоса безопасности слева
        /// </summary>
        [JsonPropertyName("b_lp")]
        public double? BLp { get; set; }

        /// <summary>
        /// Полоса безопасности справа
        /// </summary>
        [JsonPropertyName("b_pb")]
        public double? BPb { get; set; }

        /// <summary>
        /// Количество полос движения на данном интервале
        /// </summary>
        [JsonPropertyName("k_polos")]
        public int KolichestvoPolos { get; set; }

        /// <summary>
        /// Тип движения на интервале
        /// </summary>
        [JsonPropertyName("w_proezd")]
        public int ProezdType { get; set; }
    }
}
