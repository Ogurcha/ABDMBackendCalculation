namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// Напряжения от одного ТС на определённой траектории
    /// </summary>
    public class StrainsInTrajectory : ComparableStrainBase
    {
        /// <summary>
        /// Координата траектории
        /// </summary>
        public double X => VehicleTrajectoryRef.X;

        public required VehicleStrain[] Strains { get; set; }

        public TrafficJamStrain? TrafficJamStrain { get; set; }

        /// <summary>
        /// на траектории выбирается одно ТС, поэтому в формуле берём только первое напряжение
        /// </summary>
        public override double TotalStrain { get; set; }

        /// <summary>
        /// Траектория, на которой находилось ТС при замере напряжения.
        /// </summary>
        public required VehicleTrajectory VehicleTrajectoryRef { get; set; }
    }
}
