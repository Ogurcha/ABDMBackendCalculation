using System.Text.Json.Serialization;

namespace Abdm.Calculation.DAL.DataTransferObjects
{
    public class SurfaceRawDataDto
    {
        /// <summary>
        /// тип чекпоинта - балка или опора
        /// </summary>
        [JsonPropertyName("c_typnk")]
        public int c_typnk { get; set; }

        /// <summary>
        /// тип проверки на чекпоинте - зависит от типа дефформации
        /// </summary>
        [JsonPropertyName("c_cptype")]
        public int c_cptype { get; set; }

        /// <summary>
        /// лямбда - используется для расчета коеффициентов напряжения
        /// </summary>
        [JsonPropertyName("lambda")]
        public double lambda { get; set; }

        /// <summary>
        /// Бинарник с точками поверхности
        /// </summary>
        [JsonPropertyName("data")]
        public byte[]? data { get; set; }

        /// <summary>
        /// Номер структуры (пролетного строения) сооружения, к которому отностится чекпоинт
        /// </summary>
        [JsonPropertyName("substructureId")]
        public int substructureId { get; set; }
    }
}
