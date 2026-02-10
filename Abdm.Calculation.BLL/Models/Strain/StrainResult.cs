using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// Результирующее напряжение по заданному интервалу и по задданым правилам движения
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
        public required VehicleStrainList Strain { get; set; }

        /// <summary>
        /// Максимальное напряжение по каждой траектории движения в заданном интервале, если проезжает по 1 авто
        /// Необходимо для случая проверки <see cref="PassTypeEnum.SingleAutoOnly"/>
        /// </summary>
        public required VehicleStrain StrainOneAuto { get; set; }
    }
}
