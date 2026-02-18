using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.RequestModels
{
    /// <summary>
    /// реквест сообщение для начала расчётов
    /// </summary>
    public class PassTypeCalculationRequest2
    {
        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        [JsonPropertyName("c_isso")]
        public long CIsso { get; set; }

        /// <summary>
        /// Номер чекпоинта данного сооружения
        /// </summary>
        [JsonPropertyName("number")]
        public int Number { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        [JsonPropertyName("c_nagruzka")]
        public int CNagruzka { get; set; }

        /// <summary>
        /// номер выбранного снипа, по которому пойдут расчет
        /// </summary>
        [JsonPropertyName("snip")]
        public int Snip { get; set; }

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        [JsonPropertyName("direction")]
        public int Direction { get; set; }

        /// <summary>
        /// Подробные характеристики нагрузки на данное сооружение
        /// </summary>
        [JsonPropertyName("load_schema")]
        public LoadSchemaRequestModel? LoadSchema { get; set; }

        /// <summary>
        /// Характеристики "поверхности влияния" иссо
        /// </summary>
        [JsonPropertyName("surface")]
        public SurfaceRequestModel? Surface { get; set; }

        /// <summary>
        /// Характеристики пути
        /// </summary>
        [JsonPropertyName("roadway")]
        public RoadwayRequestModel? Roadway { get; set; }

        /// <summary>
        /// для прицепов, вагонов и т.п.
        /// </summary>
        [JsonPropertyName("secondary_load_schema")]
        public LoadSchemaRequestModel? SecondaryLoadSchema { get; set; }
    }
}
