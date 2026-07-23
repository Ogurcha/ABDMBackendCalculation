namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// Результирующее напряжение по заданному интервалу и по задданым правилам движения
    /// Уникально для каждой пары <see cref="RoadRule"/> и <see cref="IntervalModel"/>
    /// </summary>
    public class StrainResult 
    {
        /// <summary>
        /// Напряжение по каким правилам были посчитаны
        /// </summary>
        public required RoadRule RoadRuleRef { get; set; }

        /// <summary>
        /// Максимальное напряжение по каждой траектории движения в заданном интервале
        /// </summary>
        public required VehicleColumnStrain[] VehicleColumnStrains { get; set; }

        /// <summary>
        /// Итоговое напряжение с учётом коэффициента
        /// </summary>
        public double TotalStrain => VehicleColumnStrains.Sum(x => x.TotalStrain);
    }
}
