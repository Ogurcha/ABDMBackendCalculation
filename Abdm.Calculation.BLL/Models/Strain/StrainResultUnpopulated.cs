namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// Драфтовое напряжение по заданному интервалу и по задданым правилам движения. 
    /// Автоколонны в нём содержат только 1 ТС. 
    /// </summary>
    public class StrainResultUnpopulated 
    {
        /// <summary>
        /// Напряжение по каким правилам были посчитаны
        /// </summary>
        public required RoadRule RoadRuleRef { get; set; }

        /// <summary>
        /// Максимальное напряжение по каждой траектории движения в заданном интервале
        /// </summary>
        public required StrainsInMaximums[] Strain { get; set; }

        public required IntervalModel IntervalModelRef { get;  set; }
    }
}
