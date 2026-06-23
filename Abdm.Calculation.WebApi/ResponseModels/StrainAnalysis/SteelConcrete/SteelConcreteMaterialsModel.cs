using System.Text.Json.Serialization;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class SteelConcreteMaterialsModel
    {
        /// <summary>
        /// Steel modulus of elasticity, MPa.
        /// (Модуль упругости стали, МПа)
        /// </summary>
        [JsonPropertyName("steelElasticModulus")]
        public decimal SteelElasticModulus { get; set; }

        /// <summary>
        /// Reinforcement (rebar) modulus of elasticity, MPa.
        /// (Модуль упругости арматуры, МПа)
        /// </summary>
        [JsonPropertyName("reinforcementElasticModulus")]
        public decimal ReinforcementElasticModulus { get; set; }

        /// <summary>
        /// Concrete modulus of elasticity, MPa.
        /// (Модуль упругости бетона)
        /// </summary>
        [JsonPropertyName("concreteElasticModulus")]
        public decimal ConcreteElasticModulus { get; set; }

        /// <summary>
        /// Conversion coefficients (array).
        /// (Коэффициенты приведения)
        /// </summary>
        [JsonPropertyName("conversionCoefficientFirst")]
        public decimal ConversionCoefficientFirst { get; set; }

        /// <summary>
        /// Conversion coefficients (array).
        /// (Коэффициенты приведения)
        /// </summary>
        [JsonPropertyName("conversionCoefficientSecond")]
        public decimal ConversionCoefficientSecond { get; set; }

        /// <summary>
        /// Ultimate compressive strain of concrete.
        /// (Предельная деформация бетона на сжатие)
        /// </summary>
        [JsonPropertyName("concreteUltimateCompressiveStrain")]
        public decimal ConcreteUltimateCompressiveStrain { get; set; }

        /// <summary>
        /// Concrete modulus of elasticity used for shrinkage calculations, MPa.
        /// (Модуль упругости бетона для расчета усадки)
        /// </summary>
        [JsonPropertyName("concreteElasticModulusForShrinkage")]
        public decimal ConcreteElasticModulusForShrinkage { get; set; }

        /// <summary>
        /// NB conversion coefficients (array).
        /// (Коэффициенты приведения nb)
        /// </summary>
        [JsonPropertyName("nbConversionCoefficientFirst")]
        public decimal NbConversionCoefficientFirst { get; set; }

        /// <summary>
        /// NB conversion coefficients (array).
        /// (Коэффициенты приведения nb)
        /// </summary>
        [JsonPropertyName("nbConversionCoefficientSecond")]
        public decimal NbConversionCoefficientSecond { get; set; }

        /// <summary>
        /// Ultimate concrete strain for shrinkage calculations.
        /// (Предельная деформация бетона при расчете на усадку)
        /// </summary>
        [JsonPropertyName("concreteUltimateStrainForShrinkage")]
        public decimal ConcreteUltimateStrainForShrinkage { get; set; }

        /// <summary>
        /// Design strength of upper steel (top flange), MPa.
        /// (Расчетное сопротивление стали верхнего пояса, МПа)
        /// </summary>
        [JsonPropertyName("upperSteelDesignStrength")]
        public decimal UpperSteelDesignStrength { get; set; }

        /// <summary>
        /// Design strength of lower steel (bottom flange), MPa.
        /// (Расчетное сопротивление стали нижнего пояса, МПа)
        /// </summary>
        [JsonPropertyName("lowerSteelDesignStrength")]
        public decimal LowerSteelDesignStrength { get; set; }

        /// <summary>
        /// Design strength of slab reinforcement, MPa.
        /// (Расчетное сопротивление арматуры плиты, МПа)
        /// </summary>
        [JsonPropertyName("slabReinforcementDesignStrength")]
        public decimal SlabReinforcementDesignStrength { get; set; }

        /// <summary>
        /// Design strength of concrete, MPa.
        /// (Расчетное сопротивление бетона, МПа)
        /// </summary>
        [JsonPropertyName("concreteDesignStrength")]
        public decimal ConcreteDesignStrength { get; set; }

        /// <summary>
        /// Maximum temperature difference, °C.
        /// (Максимальная разность температур)
        /// </summary>
        [JsonPropertyName("maximumTemperatureDifference")]
        public decimal MaximumTemperatureDifference { get; set; }

        /// <summary>
        /// Moment of inertia of the concrete slab, m^4.
        /// (Момент инерции бетонной плиты, м4)
        /// </summary>
        [JsonPropertyName("concreteSlabMomentOfInertia")]
        public decimal ConcreteSlabMomentOfInertia { get; set; }

        /// <summary>
        /// Indicates whether the thin-plate method is applicable.
        /// (Применимость метода тонкой плиты)
        /// </summary>
        [JsonPropertyName("thinPlateMethodApplicable")]
        public bool ThinPlateMethodApplicable { get; set; }
    }
}

