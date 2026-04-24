using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Default;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Models.StrainAnalysis
{
    /// <summary>
    /// Модель анализа напряжения. Содержит подробную информацию о наименее выгодной для сооружения прокатке транспортных средств.
    /// </summary>
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
        /// Проектная устойчивость структуры. Без учёта собственного веса
        /// </summary>
        public decimal MyStrength { get; set; }

        /// <summary>
        /// Фиксированная нагрузка от собственного веса
        /// </summary>
        public decimal ConstLoad { get; set; }

        /// <summary>
        /// Нагрузка от пешеходов
        /// </summary>
        public decimal PedestrianLoad { get; set; }

        /// <summary>
        /// Другая нагрузка
        /// </summary>
        public decimal OtherLoad { get; set; }

        /// <summary>
        /// Результат анализа при обычном расчёте
        /// </summary>
        public List<AnalysisDefault>? Default { get; set; }
    }
}
