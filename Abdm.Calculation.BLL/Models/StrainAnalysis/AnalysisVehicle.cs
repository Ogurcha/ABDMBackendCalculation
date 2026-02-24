namespace Abdm.Calculation.BLL.Models.StrainAnalysis
{
    public class AnalysisVehicle
    {
        public required List<AnalysisWheel> Axles { get; set; }

        public List<AnalysisPositiveInterval>? AnalysisPositiveIntervals { get; set; }
    }
}
