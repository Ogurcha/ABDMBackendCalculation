using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Models.Algorithmic
{
    /// <summary>
    /// Общая модель для расчета напряжения и условий пропуска
    /// </summary>
    public class PassTypeDataModel
    {
        /// <summary>
        /// Модель интервалов движения
        /// </summary>
        public required List<IntervalModel> Intervals { get; set; }

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
