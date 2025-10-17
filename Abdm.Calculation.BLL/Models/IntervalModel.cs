namespace Abdm.Calculation.BLL.Models
{

    public class IntervalModel
    {
        /// <summary>
        /// Напряжение по какому интервалу было посичтано
        /// </summary>
        public required PassageInterval PassageIntervalRef { get; set; }

        /// <summary>
        /// Физические траектории движения ТС в заданноом интервале
        /// </summary>
        public VehicleTrajectory[] Trajectories { get; set; } = [];
    }
}
