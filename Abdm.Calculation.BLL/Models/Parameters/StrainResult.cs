using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models.Parameters
{
    /// <summary>
    /// Результирующее напряжение по заданному интервалу и по задданым правилам движения
    /// </summary>
    public class StrainResult 
    {
        public StrainResult([DisallowNull] PassageInterval passageInterval, [DisallowNull] RoadRule roadRule) 
        {
            PassageIntervalRef = passageInterval;
            RoadRuleRef = roadRule;
        }

        /// <summary>
        /// Напряжение по каким правилам были посчитаны
        /// </summary>
        public RoadRule RoadRuleRef { get; set; }

        /// <summary>
        /// Напряжение по какому интервалу было посичтано
        /// </summary>
        public PassageInterval PassageIntervalRef { get; set; }

        /// <summary>
        /// Максимальное напряжение по каждой траектории движения в заданном интервале
        /// </summary>
        public double Strain { get; set; }

        /// <summary>
        /// Максимальное напряжение по каждой траектории движения в заданном интервале, если проезжает по 1 авто
        /// Необходимо для случая проверки <see cref="PassTypeEnum.SingleAutoOnly"/>
        /// </summary>
        public double StrainOneAuto { get; set; }
    }
}
