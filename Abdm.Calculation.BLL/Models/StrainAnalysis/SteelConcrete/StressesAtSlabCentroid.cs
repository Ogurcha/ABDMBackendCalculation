namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class StressesAtSlabCentroid
    {
        /// <summary>
        /// Напряжения в бетоне на уровне центра тяжести плиты от постоянных нагрузок 2 стадии и временных нагрузок при максимально допустимых классах, МПа
        /// Concrete stresses at the slab centroid level from permanent loads (stage 2) and temporary loads at maximum permissible classes, MPa.
        /// </summary>
        public decimal ConcreteStresses { get; set; }

        /// <summary>
        /// Контрольное значение для бетона, МПа
        /// Control (limit) value for concrete, MPa.
        /// </summary>
        public decimal ConcreteControlValue { get; set; }

        /// <summary>
        /// Напряжения в арматуре на уровне центра тяжести плиты от постоянных нагрузок 2 стадии и временных нагрузок при максимально допустимых классах, МПа
        /// Reinforcement stresses at the slab centroid level from permanent loads (stage 2) and temporary loads at maximum permissible classes, MPa.
        /// </summary>
        public decimal ReinforcementStresses { get; set; }

        /// <summary>
        /// Контрольное значение для арматуры, МПа
        /// Control (limit) value for reinforcement, MPa.
        /// </summary>
        public decimal ReinforcementControlValue { get; set; }

        /// <summary>
        /// Расчетный случай (А/Б/В)
        /// Calculation case (A/B/C) — represented as string (e.g. "A", "B", "C").
        /// </summary>
        public required string CalculationCase { get; set; }

        /// <summary>
        /// Разгружающая сила, МН
        /// Unloading (relief) force, MN.
        /// </summary>
        public decimal UnloadingForce { get; set; }

        /// <summary>
        /// Напряжения в верхнем поясе от постоянных нагрузок (по стадиям), МПа
        /// Stresses in the upper flange from permanent loads (by stages), MPa.
        /// </summary>
        public decimal UpperFlangeStresses { get; set; }

        /// <summary>
        /// Напряжения в верхнем поясе от постоянных нагрузок (по стадиям), МПа
        /// Stresses in the upper flange from permanent loads (by stages), MPa.
        /// </summary>
        public decimal UpperFlangeStresses2 { get; set; }

        /// <summary>
        /// Напряжения в нижнем поясе от постоянных нагрузок (по стадиям), МПа
        /// Stresses in the lower flange from permanent loads (by stages), MPa.
        /// </summary>
        public decimal LowerFlangeStresses { get; set; }

        /// <summary>
        /// Напряжения в нижнем поясе от постоянных нагрузок (по стадиям), МПа
        /// Stresses in the lower flange from permanent loads (by stages), MPa.
        /// </summary>
        public decimal LowerFlangeStresses2 { get; set; }
    }
}
