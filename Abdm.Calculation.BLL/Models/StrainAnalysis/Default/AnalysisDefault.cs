namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class AnalysisDefault()
    {
        public required bool? HasSafetyLine { get; set; }

        public required bool IsForward { get; set; }

        public required AnalysisColumn[] Columns { get; set; }
    }
}
