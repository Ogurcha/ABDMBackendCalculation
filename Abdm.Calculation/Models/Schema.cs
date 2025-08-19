namespace Abdm.Calculation.Models
{
    /// <summary>
    /// Подробные характеристики нагрузки на сооружение
    /// ais7Nagruzka в старом клиенте
    /// </summary>
    public class Schema
    {
        /// <summary>
        /// идентификатор нагрузки
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ais7PcNType в старом клиенте
        /// </summary>
        public string Type_id { get; set; } //"10"

        public string Type { get; set; } //"Колесная общего назначения"

        /// <summary>
        /// Название-аббревиатура данной нагрузки
        /// </summary>
        public string Name { get; set; } //"ЭНз"

        public float Width { get; set; } //2.9

        public float Length { get; set; } //7.4

        public float? Distance { get; set; } //10.0

        /// <summary>
        /// ais7NagruzkaAxle в старом клиенте
        /// </summary>
        public Axle[] Axles { get; set; }
    }
}
