namespace Abdm.Calculation.BLL.Models
{
    public class VehicleTrajectoryFilter
    {
        public required Func<double, bool> Filter { get; set; }

        public required RoadRule RoadRuleRef { get; set; }

        public double EdgeCaseLeft { get; set; }

        public double EdgeCaseRight { get; set; }
    }
}
