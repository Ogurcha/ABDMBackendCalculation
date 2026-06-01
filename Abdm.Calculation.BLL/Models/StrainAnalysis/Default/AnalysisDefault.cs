using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class AnalysisDefault()
    {
        public required bool? HasSafetyLine { get; set; }

        public required bool IsForward { get; set; }

        public required AnalysisColumn[] Columns { get; set; }

        public PassageIntervalTypeEnum IntervalType { get; set; }

        /// <summary>
        /// Положение ограждения слева
        /// </summary>
        public decimal BarrierPositionLeft { get; set; }

        /// <summary>
        /// Положение ограждения справа
        /// </summary>
        public decimal BarrierPositionRight { get; set; }
    }
}
