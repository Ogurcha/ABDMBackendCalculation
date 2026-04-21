using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Default;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Models.StrainAnalysis
{
    public class AnalysisSummary
    {
        /// <summary>
        /// Тип расчета напряжений. Влияет на выбор стратегии анализа и на структуру результирующей модели
        /// </summary>
        public StrainCalculationGroupTypeEnum StrainCalculationGroupType { get; set; }

        /// <summary>
        /// Тип напряжения не сгруппированный по типу расчёта
        /// </summary>
        public StrainCalculationTypeEnum StrainCalculationType { get; set; }

        /// <summary>
        /// Положение ограждения слева
        /// </summary>
        public decimal AbsolutePositionLeft { get; set; }

        /// <summary>
        /// Положение ограждения справа
        /// </summary>
        public decimal AbsolutePositionRight { get; set; }

        /// <summary>
        /// лямбда - используется для расчета коеффициентов напряжения
        /// </summary>
        public decimal Lambda { get; set; }

        /// <summary>
        /// Результат при обычном расчете (железобетон)
        /// </summary>
        public List<AnalysisDefault>? Default { get; set; }

        ///// <summary>
        ///// Результат при расчёте по опорам
        ///// </summary>
        //public List<AnalysisPillar>? Pillar { get; set; }


        ///// <summary>
        ///// Результат при расчёте по стжб
        ///// </summary>
        //public List<AnalysisSteelConcrete>? SteelConcrete { get; set; }
    }
}
