namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class AnalysisDefault
    {
        public int ColumnNumber { get; set; }

        public required List<WheelAnalysis> Wheels { get; set; }

        public List<TrafficJamStrainAnalysis>? Intervals { get; set; }
    }
}
