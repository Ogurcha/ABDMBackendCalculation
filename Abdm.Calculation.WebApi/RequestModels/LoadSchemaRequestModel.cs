using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.RequestModels
{
    /// <summary>
    /// Подробные характеристики нагрузки на сооружение
    /// </summary>
    public class LoadSchemaRequestModel
    {
        /// <summary>
        /// DTO идентификатора нагрузки
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Тип нагрузки
        /// </summary>
        [JsonPropertyName("type_id")]
        public int? TypeId { get; set; }

        /// <summary>
        /// Название типа нагрузки
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Название-аббревиатура данной нагрузки
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Нормативный отступ от края. Больше нуля, если без заезда на полосу. Ноль - если с заездом на ограничительную полосу
        /// </summary>
        [JsonPropertyName("width")]
        public double? Width { get; set; }

        /// <summary>
        /// Длина
        /// </summary>
        [JsonPropertyName("length")]
        public double? Length { get; set; }

        /// <summary>
        /// Расстояние
        /// </summary>
        [JsonPropertyName("distance")]
        public double? Distance { get; set; }

        /// <summary>
        /// Точки давления, которые представляют данную нагрузку
        /// Например, массив из 4 колес
        /// </summary>
        [JsonPropertyName("axles")]
        public AxleRequestModel[]? Axles { get; set; }
    }
}
