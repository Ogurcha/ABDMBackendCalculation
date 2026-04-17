using System.Collections.Generic;
using System.Text.Json.Serialization;
using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default;
using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Pillar;
using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis
{
    public class AnalysisSummaryModel
    {
        /// <summary>
        /// Тип расчета напряжений. Влияет на выбор стратегии анализа и на структуру результирующей модели
        /// </summary>
        [JsonPropertyName("calculationType")]
        public int CalculationType { get; set; }

        /// <summary>
        /// Положение ограждения слева
        /// </summary>
        [JsonPropertyName("absolutePositionLeft")]
        public decimal AbsolutePositionLeft { get; set; }

        /// <summary>
        /// Положение ограждения справа
        /// </summary>
        [JsonPropertyName("absolutePositionRight")]
        public decimal AbsolutePositionRight { get; set; }

        /// <summary>
        /// Результат при обычном расчете (железобетон)
        /// </summary>
        [JsonPropertyName("default")]
        public List<AnalysisDefaultModel>? Default { get; set; }

        /// <summary>
        /// Результат при расчёте по опорам
        /// </summary>
        [JsonPropertyName("pillar")]
        public List<AnalysisPillarModel>? Pillar { get; set; }

        ///// <summary>
        ///// Результат при расчёте по стжб
        ///// </summary>
        //[JsonPropertyName("steelConcrete")]
        //public List<AnalysisSteelConcreteModel>? SteelConcrete { get; set; }

        [JsonPropertyName("cptype")]
        public int StrainCalculationType { get; set; }
    }
}
