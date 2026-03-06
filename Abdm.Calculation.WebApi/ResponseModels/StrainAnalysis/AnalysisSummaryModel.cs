using System.Collections.Generic;
using System.Text.Json.Serialization;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Default;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Pillar;
using Abdm.Calculation.BLL.Models.StrainAnalysis.SteelConcrete;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis
{
    public class AnalysisSummaryModel
    {
        /// <summary>
        /// Тип расчета напряжений. Влияет на выбор стратегии анализа и на структуру результирующей модели
        /// </summary>
        [JsonPropertyName("calculationType")]
        public StrainCalculationGroupTypeEnum CalculationType { get; set; }

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
        public List<AnalysisDefault>? Default { get; set; }

        /// <summary>
        /// Результат при расчёте по опорам
        /// </summary>
        [JsonPropertyName("pillar")]
        public List<AnalysisPillar>? Pillar { get; set; }

        /// <summary>
        /// Результат при расчёте по стжб
        /// </summary>
        [JsonPropertyName("steelConcrete")]
        public List<AnalysisSteelConcrete>? SteelConcrete { get; set; }
    }
}
