using System.Collections.Generic;
using System.Text.Json.Serialization;
using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.Default;
using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete;

namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis
{
    /// <summary>
    /// Модель анализа напряжения. Содержит подробную информацию о наименее выгодной для сооружения прокатке транспортных средств.
    /// </summary>
    public class AnalysisSummaryModel
    {
        /// <summary>
        /// Тип расчета напряжений. Влияет на выбор стратегии анализа и на структуру результирующей модели
        /// </summary>
        [JsonPropertyName("calculationType")]
        public int StrainCalculationGroupType { get; set; }

        /// <summary>
        /// Тип напряжения не сгруппированный по типу расчёта
        /// </summary>
        [JsonPropertyName("cptype")]
        public int StrainCalculationType { get; set; }

        /// <summary>
        /// Положение ограждения слева
        /// </summary>
        [JsonPropertyName("absolutePositionFarLeft")]
        public decimal AbsolutePositionFarLeft { get; set; }

        /// <summary>
        /// Положение ограждения справа
        /// </summary>
        [JsonPropertyName("absolutePositionFarRight")]
        public decimal AbsolutePositionFarRight { get; set; }

        /// <summary>
        /// лямбда - используется для расчета коеффициентов напряжения
        /// </summary>
        [JsonPropertyName("lambda")]
        public decimal Lambda { get; set; }

        /// <summary>
        /// Результат при обычном расчете (железобетон)
        /// </summary>
        [JsonPropertyName("default")]
        public List<AnalysisDefaultModel>? Default { get; set; }

        /// <summary>
        /// Дополнительные результаты при расчете для ЖБ конструкций
        /// </summary>
        [JsonPropertyName("steelConcrete")]
        public AnalysisSteelConcreteModel? SteelConcrete { get; set; }

        /// <summary>
        /// Проектная устойчивость структуры. Без учёта собственного веса
        /// </summary>
        [JsonPropertyName("myStrength")]
        public decimal MyStrength { get; set; }

        /// <summary>
        /// Фиксированная нагрузка от собственного веса
        /// </summary>
        [JsonPropertyName("constLoad")]
        public decimal ConstLoad { get; set; }

        /// <summary>
        /// Нагрузка от пешеходов
        /// </summary>
        [JsonPropertyName("pedestrianLoad")]
        public decimal PedestrianLoad { get; set; }

        /// <summary>
        /// Другая нагрузка
        /// </summary>
        [JsonPropertyName("otherLoad")]
        public decimal OtherLoad { get; set; }

        [JsonPropertyName("barrierInfo")]
        public BarrierInfoModel BarrierInfo { get; set; }
    }
}
