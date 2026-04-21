namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class AnalysisVehicle
    {
        public int ColumnNumber { get; set; }

        public int VehicleNumber { get; set; }

        public decimal PositionX { get; set; }

        public decimal PositionY { get; set; }

        public required List<WheelAnalysis> Wheels { get; set; }

        public decimal SumStrain { get; set; }

        public decimal TotalStrain { get; set; }

        public List<TrafficJamStrainAnalysis>? Intervals { get; set; }

        public ProfileVector[]? IntervalProfileVectors { get; set; }

        public decimal LambdaSmall { get; set; }

        public decimal DynamicCoefficient { get; set; }
    }
}
