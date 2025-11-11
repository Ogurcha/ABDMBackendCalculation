using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Уменьшенная модель расчета напряжения и условий пропуска
    /// </summary>
    public class PassTypeSmallModel
    {
        public DriveDirectionEnum Direction { get; set; } = DriveDirectionEnum.Bidirection;

        /// <summary>
        /// Модель поверхности, по которой едет ТС
        /// </summary>
        public required SurfaceModel Surface { get; set; }

        /// <summary>
        /// Модель нагрузки
        /// </summary>
        public required LoadModel Load { get; set; }
    }
}
