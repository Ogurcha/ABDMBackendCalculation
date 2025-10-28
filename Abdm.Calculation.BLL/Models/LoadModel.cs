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
        public required Axle[] Axles { get; set; }

        /// <summary>
        /// Индентичен ли перед нагрузки с его задом
        /// </summary>
        public bool? IsSymmetric { get; set; }

        /// <summary>
        /// минимальное расстояние между транспортными колоннами
        /// </summary>
        public double Interval { get; internal set; } = NormConstants.MinimalDistanceBetweenTrajectories;

        /// <summary>
        /// Для прицепов, вагонов поезда и т.п.
        /// </summary>
        public LoadModel? SecondaryLoadModel { get; set; }
    }
}
