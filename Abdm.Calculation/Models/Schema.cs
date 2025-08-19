namespace Abdm.Calculation.Models
{
    /// <summary>
    /// Подробные характеристики нагрузки на сооружение
    /// </summary>
    public class Schema
    {
        /// <summary>
        /// идентификатор нагрузки
        /// </summary>
        public long Id { get; set; }

        public string Type_id { get; set; } //"10"

        public string Type { get; set; } //"Колесная общего назначения"

        /// <summary>
        /// Название-аббревиатура данной нагрузки
        /// </summary>
        public string Name { get; set; } //"ЭНз"

        public float Width { get; set; } //2.9

        public float Length { get; set; } //7.4

        public float Distance { get; set; } //10.0

        public Axle[] Axles { get; set; }
    }
}
