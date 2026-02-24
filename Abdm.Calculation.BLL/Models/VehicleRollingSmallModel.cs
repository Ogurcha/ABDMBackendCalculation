using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Уменьшенная модель расчета напряжения и условий пропуска
    /// </summary>
    public class VehicleRollingSmallModel
    {
        public DriveDirectionEnum Direction { get; set; } = DriveDirectionEnum.Bidirection;

        /// <summary>
        /// Модель поверхности, по которой едет ТС
        /// </summary>
        [NotNull]
        public required SurfaceModel Surface { get; set; }

        /// <summary>
        /// Модель нагрузки
        /// </summary>
        [NotNull]
        public required LoadModel Load { get; set; }
    }
}
