namespace Abdm.Calculation.BLL.Models.StrainAnalysis
{
    public class AnalysisVehicle
    {
        public int ColumnNumber { get; set; }

        public required List<AnalysisWheel> Wheels { get; set; }

        public List<AnalysisPositiveInterval>? AnalysisPositiveIntervals { get; set; }
    }
}
