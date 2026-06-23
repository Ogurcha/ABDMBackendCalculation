using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class CreepAccountingModel
    {
        /// <summary>
        /// Напряжения на уровне центра тяжести плиты от M2g, МПа
        /// Stresses at the slab centroid level from M2g, MPa.
        /// </summary>
        [JsonPropertyName("stressesAtSlabCentroidFromM2g")]
        public decimal StressesAtSlabCentroidFromM2g { get; set; }

        /// <summary>
        /// Контрольное значение, МПа
        /// Control (limit) value, MPa.
        /// </summary>
        [JsonPropertyName("controlValue")]
        public decimal ControlValue { get; set; }

        /// <summary>
        /// Учет ползучести бетона не требуется да/нет
        /// Indicates whether accounting for slab concrete creep is not required (true = not required, false = required).
        /// </summary>
        [JsonPropertyName("creepAccountingNotRequired")]
        public bool CreepAccountingNotRequired { get; set; }

        /// <summary>
        /// Напряжения от ползучести бетона в плите, МПа
        /// Stresses in the slab due to concrete creep, MPa.
        /// </summary>
        [JsonPropertyName("stressesFromConcreteCreepInSlab")]
        public decimal StressesFromConcreteCreepInSlab { get; set; }

        /// <summary>
        /// Напряжения от ползучести бетона в арматуре, МПа
        /// Stresses in the reinforcement due to concrete creep, MPa.
        /// </summary>
        [JsonPropertyName("stressesFromConcreteCreepInReinforcement")]
        public decimal StressesFromConcreteCreepInReinforcement { get; set; }
    }
}
