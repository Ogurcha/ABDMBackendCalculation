namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Pillar
{
    public class AnalysisPillar
    {
        public int ColumnNumber { get; set; }

        public required bool IsForward { get; set; }

        public required List<AxleAnalysis> Axles { get; set; }

        public List<TrafficJamStrainAnalysisSlim>? Intervals { get; set; }
    }
}
