using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Рассчитанные разные варианты напряжения на выбранном интервале при выбранных правилах движения
    /// </summary>
    public class StrainMap
    {
        /// <summary>
        /// Напряжение по каким правилам были посчитаны
        /// </summary>
        public required RoadRule RoadRuleRef { get; set; }

        /// <summary>
        /// Выбранный интервал, на котором исследуются напряжения
        /// </summary>
        public required IntervalModel IntervalModelRef { get; set; }

        public required StrainsInMaximums[] StrainsInMaximums { get; set; }
    }
}
