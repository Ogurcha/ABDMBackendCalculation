namespace Abdm.Calculation.Models
{
    /// <summary>
    /// Подробные характеристики нагрузки на сооружение
    /// </summary>
    public class NagruzkaSchema
    {
        /// <summary>
        /// идентификатор нагрузки
        /// </summary>
        public NagruzkaEnum Id { get; set; }

        /// <summary>
        /// Тип нагрузки
        /// </summary>
        public NagruzkaTypeEnum Type { get; set; }

        /// <summary>
        /// Название типа нагрузки
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Название-аббревиатура данной нагрузки
        /// </summary>
        public string NameShort { get; set; }

        /// <summary>
        /// Ширина
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Длина
        /// </summary>
        public float Length { get; set; }

        /// <summary>
        /// Расстояние
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// Точки давления, которые представляют данную нагрузку
        /// Например, массив из 4 колес
        /// </summary>
        public Axle[] Axles { get; set; }
    }
}
