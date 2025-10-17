using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Модель нагрузки
    /// </summary>
    public class LoadModel
    {
        public DriveDirectionEnum Direction { get; set; } = DriveDirectionEnum.Bidirection;

        /// <summary>
        /// Ширина ТС
        /// </summary>
        public required double Width { get; set; }

        /// <summary>
        /// Длина ТС
        /// </summary>
        public required double Length { get; set; }

        /// <summary>
        /// Расстояние между ТС
        /// </summary>
        public required double Distance { get; set; }

        /// <summary>
        /// Оси ТС
        /// </summary>
        public required Axle[] Axles { get; set; }

    }
}
