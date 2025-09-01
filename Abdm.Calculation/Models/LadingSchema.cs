namespace Abdm.Calculation.Models
{
    /// <summary>
    /// Подробные характеристики нагрузки на сооружение
    /// </summary>
    public class LadingSchema
    {
        /// <summary>
        /// идентификатор нагрузки
        /// </summary>
        public LadingEnum Id { get; set; }

        /// <summary>
        /// Тип нагрузки
        /// </summary>
        public LadingGroupTypeEnum Type { get; set; }

        /// <summary>
        /// Название типа нагрузки
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Название-аббревиатура данной нагрузки
        /// </summary>
        public string NameShort { get; set; }

        /// <summary>
        /// Нормативный отступ от края. Больше нуля, если без заезда на полосу. Ноль - если с заездом на ограничительную полосу
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Длина
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Расстояние
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Точки давления, которые представляют данную нагрузку
        /// Например, массив из 4 колес
        /// </summary>
        public Axle[] Axles { get; set; }
    }
}
