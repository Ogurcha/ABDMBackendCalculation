using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Модель нагрузки
    /// </summary>
    public class LoadModel
    {
        /// <summary>
        /// Ширина ТС
        /// </summary>
        public required double Width { get; set; }

        /// <summary>
        /// Длина ТС
        /// </summary>
        public required double Length { get; set; }

        /// <summary>
        /// Расстояние до следующего ТС в транспорнтной колонне
        /// </summary>
        public required double Distance { get; set; }

        /// <summary>
        /// Оси ТС
        /// </summary>
        [NotNull]
        public required Axle[] Axles { get; set; }

        /// <summary>
        /// Индентичен ли перед нагрузки с его задом
        /// </summary>
        public bool IsSymmetric { get; set; }

        /// <summary>
        /// Направления для прокатки ТС. true - вперед, false - назад. true+false = оба.
        /// </summary>
        public bool[] ActualDirection { get; set; } = [true];

        /// <summary>
        /// минимальное расстояние между транспортными колоннами
        /// </summary>
        public double Interval { get; set; } = NormConstants.MinimalDistanceBetweenTrajectories;

        /// <summary>
        /// Для прицепов, вагонов поезда и т.п.
        /// </summary>
        public LoadModel? SecondaryLoadModel { get; set; }

        /// <summary>
        /// Тип нагрузки
        /// </summary>
        public LoadGroupTypeEnum Type { get; set; }
    }
}
