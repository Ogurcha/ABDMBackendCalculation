namespace Abdm.Calculation.WebApi.RequestModels
{
    /// <summary>
    /// Подробные характеристики нагрузки на сооружение
    /// </summary>
    public class LadingSchemaRequestModel
    {
        /// <summary>
        /// DTO идентификатора нагрузки
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// Тип нагрузки
        /// </summary>
        public string? type_id { get; set; }

        /// <summary>
        /// Название типа нагрузки
        /// </summary>
        public string? type { get; set; }

        /// <summary>
        /// Название-аббревиатура данной нагрузки
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// Нормативный отступ от края. Больше нуля, если без заезда на полосу. Ноль - если с заездом на ограничительную полосу
        /// </summary>
        public double width { get; set; }

        /// <summary>
        /// Длина
        /// </summary>
        public double length { get; set; }

        /// <summary>
        /// Расстояние
        /// </summary>
        public double distance { get; set; }

        /// <summary>
        /// Точки давления, которые представляют данную нагрузку
        /// Например, массив из 4 колес
        /// </summary>
        public AxleRequestModel[]? axles { get; set; }
    }
}
