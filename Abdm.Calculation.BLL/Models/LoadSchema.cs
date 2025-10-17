using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Подробные характеристики нагрузки на сооружение
    /// </summary>
    public class LoadSchema
    {
        /// <summary>
        /// идентификатор нагрузки
        /// </summary>
        public LoadEnum Id { get; set; }

        /// <summary>
        /// Тип нагрузки
        /// </summary>
        public LoadGroupTypeEnum Type { get; set; }

        /// <summary>
        /// Название типа нагрузки
        /// </summary>
        public required string TypeName { get; set; }

        /// <summary>
        /// Название-аббревиатура данной нагрузки
        /// </summary>
        public required string NameShort { get; set; }

        /// <summary>
        /// Ширина ТС
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// Длина ТС
        /// </summary>
        public double? Length { get; set; }

        /// <summary>
        /// Расстояние между ТС
        /// </summary>
        public double? Distance { get; set; }

        /// <summary>
        /// Оси ТС
        /// </summary>
        public required Axle[] Axles { get; set; }
    }
}
