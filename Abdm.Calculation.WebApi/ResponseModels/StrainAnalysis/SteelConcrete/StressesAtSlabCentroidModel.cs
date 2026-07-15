using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class StressesAtSlabCentroidModel
    {
        /// <summary>
        /// Напряжения в бетоне на уровне центра тяжести плиты от постоянных нагрузок 2 стадии и временных нагрузок при максимально допустимых классах, МПа
        /// Concrete stresses at the slab centroid level from permanent loads (stage 2) and temporary loads at maximum permissible classes, MPa.
        /// </summary>
        [JsonPropertyName("concreteStresses")]
        public decimal ConcreteStresses { get; set; }

        /// <summary>
        /// Контрольное значение для бетона, МПа
        /// Control (limit) value for concrete, MPa.
        /// </summary>
        [JsonPropertyName("concreteControlValue")]
        public decimal ConcreteControlValue { get; set; }

        /// <summary>
        /// Напряжения в арматуре на уровне центра тяжести плиты от постоянных нагрузок 2 стадии и временных нагрузок при максимально допустимых классах, МПа
        /// Reinforcement stresses at the slab centroid level from permanent loads (stage 2) and temporary loads at maximum permissible classes, MPa.
        /// </summary>
        [JsonPropertyName("reinforcementStresses")]
        public decimal ReinforcementStresses { get; set; }

        /// <summary>
        /// Контрольное значение для арматуры, МПа
        /// Control (limit) value for reinforcement, MPa.
        /// </summary>
        [JsonPropertyName("reinforcementControlValue")]
        public decimal ReinforcementControlValue { get; set; }

        /// <summary>
        /// Расчетный случай (А/Б/В)
        /// Calculation case (A/B/C) — represented as string (e.g. "A", "B", "C").
        /// </summary>
        [JsonPropertyName("calculationCase")]
        public string CalculationCase { get; set; }

        /// <summary>
        /// Разгружающая сила, МН
        /// Unloading (relief) force, MN.
        /// </summary>
        [JsonPropertyName("unloadingForce")]
        public decimal UnloadingForce { get; set; }

        /// <summary>
        /// Напряжения в верхнем поясе от постоянных нагрузок (по стадиям), МПа
        /// Stresses in the upper flange from permanent loads (by stages), MPa.
        /// </summary>
        [JsonPropertyName("upperFlangeStresses")]
        public decimal UpperFlangeStresses { get; set; }

        /// <summary>
        /// Напряжения в верхнем поясе от постоянных нагрузок (по стадиям), МПа
        /// Stresses in the upper flange from permanent loads (by stages), MPa.
        /// </summary>
        [JsonPropertyName("upperFlangeStresses2")]
        public decimal UpperFlangeStresses2 { get; set; }

        /// <summary>
        /// Напряжения в нижнем поясе от постоянных нагрузок (по стадиям), МПа
        /// Stresses in the lower flange from permanent loads (by stages), MPa.
        /// </summary>
        [JsonPropertyName("lowerFlangeStresses")]
        public decimal LowerFlangeStresses { get; set; }

        /// <summary>
        /// Напряжения в нижнем поясе от постоянных нагрузок (по стадиям), МПа
        /// Stresses in the lower flange from permanent loads (by stages), MPa.
        /// </summary>
        [JsonPropertyName("lowerFlangeStresses2")]
        public decimal LowerFlangeStresses2 { get; set; }
    }
}
