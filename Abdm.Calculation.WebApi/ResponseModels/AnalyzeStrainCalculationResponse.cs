using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels
{
    public class AnalyzeStrainCalculationResponse
    {
        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        [JsonPropertyName("issoId")]
        public long IssoId { get; set; }

        /// <summary>
        /// Номер чекпоинта данного сооружения
        /// </summary>
        [JsonPropertyName("checkPointNumber")]
        public int CheckPointNumber { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        [JsonPropertyName("loadId")]
        public long LoadId { get; set; }

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        [JsonPropertyName("direction")]
        public int Direction { get; set; }

        /// <summary>
        /// номер выбранного снипа, по которому пойдут расчет
        /// </summary>
        [JsonPropertyName("snipId")]
        public int SnipId { get; set; }

        [JsonPropertyName("summary")]
        public AnalysisSummaryModel Summary { get; set; }
        
    }
}
