using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Уменьшенная модель расчета напряжения и условий пропуска
    /// </summary>
    public class VehicleRollingSmallModel
    {
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

        /// <summary>
        /// Генератор коеффициентов напряжения
        /// </summary>
        public required ICoefficientProvider CoefficientProvider { get; set; }

        public double DynamicCoefficient() => Surface.Material == null ? NormConstants.MinStrainCoefficient
            : CoefficientProvider.GetDynamicCoefficient(Surface.Lambda, Surface.Material, Surface.StrainCalculationGroupType);
    }
}
