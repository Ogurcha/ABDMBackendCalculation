using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Default;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Pillar;
using Abdm.Calculation.BLL.Models.StrainAnalysis.SteelConcrete;

namespace Abdm.Calculation.BLL.Models.StrainAnalysis
{
    public class AnalysisSummary
    {
        /// <summary>
        /// Тип расчета напряжений. Влияет на выбор стратегии анализа и на структуру результирующей модели
        /// </summary>
        public StrainCalculationGroupTypeEnum CalculationType { get; set; }

        /// <summary>
        /// Правила, по которым была рассчитана нагрузка
        /// </summary>
        public required RoadRule RoadRule { get; set; }

        /// <summary>
        /// Результат при обычном расчете (железобетон)
        /// </summary>
        public List<AnalysisDefault>? Default { get; set; }

        /// <summary>
        /// Результат при расчёте по опорам
        /// </summary>
        public List<AnalysisPillar>? Pillar { get; set; }

        /// <summary>
        /// Результат при расчёте по стжб
        /// </summary>
        public List<AnalysisSteelConcrete>? SteelConcrete { get; set; }
    }
}
