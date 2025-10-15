using System.Text.Json.Serialization;

namespace Abdm.Calculation.DAL.DataTransferObjects
{
    public class SurfaceRawDataDto
    {
        /// <summary>
        /// тип чекпоинта - балка или опора
        /// </summary>
        [JsonPropertyName("c_typnk")]
        public int CTypnk { get; set; }

        /// <summary>
        /// тип проверки на чекпоинте - зависит от типа дефформации
        /// </summary>
        [JsonPropertyName("c_cptype")]
        public int CCptype { get; set; }

        /// <summary>
        /// лямбда - используется для расчета коеффициентов напряжения
        /// </summary>
        [JsonPropertyName("lambda")]
        public double Lambda { get; set; }

        /// <summary>
        /// Бинарник с точками поверхности
        /// </summary>
        [JsonPropertyName("data")]
        public byte[]? Data { get; set; }
    }
}
