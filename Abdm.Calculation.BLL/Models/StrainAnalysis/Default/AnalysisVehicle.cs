namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class AnalysisVehicle
    {
        public int ColumnNumber { get; set; }

        public decimal PositionX { get; set; }

        public decimal PositionY { get; set; }

        public required List<WheelAnalysis> Wheels { get; set; }

        public decimal SumStrain { get; set; }

        public List<TrafficJamStrainAnalysis>? Intervals { get; set; }

        public ProfileVector[]? IntervalProfileVectors { get; set; }
    }
}
