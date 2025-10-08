using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Models.Algorithmic
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
        /// Оси ТС
        /// </summary>
        public required Axle[] Axles { get; set; }

    }
}
