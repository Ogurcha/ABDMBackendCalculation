using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Models.Algorithmic
{
    /// <summary>
    /// Интервал, по которому идет фактическое движение ТС
    /// </summary>
    public class IntervalModel
    {
        public IntervalModel([DisallowNull] PassageInterval passageInterval, [DisallowNull] RoadRule roadRule)
        {
            PassageIntervalRef = passageInterval;
            RoadRuleRef = roadRule;
        }

        /// <summary>
        /// Напряжение по каким правилам были посчитаны
        /// </summary>
        public RoadRule RoadRuleRef { get; }

        /// <summary>
        /// Напряжение по какому интервалу было посичтано
        /// </summary>
        public PassageInterval PassageIntervalRef { get; }

        /// <summary>
        /// Расстояние между ТС
        /// </summary>
        public required double VehicleDistance { get; set; }

        /// <summary>
        /// Сколько следует отсупить слева
        /// </summary>
        public required double DistanceForSafetyLineLeft { get; set; }

        /// <summary>
        /// Сколько следует отсупить слева
        /// </summary>
        public required double DistanceForSafetyLineRight { get; set; }

        /// <summary>
        /// Абсолютное положение начала интервала
        /// </summary>
        public double AbsolutePositionLeft { get; set; }

        /// <summary>
        /// Абсолютное положение конца интервала
        /// </summary>
        public double AbsolutePositionRight { get; set; }

        /// <summary>
        /// Количество полос движения на данном интервале
        /// </summary>
        public double LaneCount { get; set; }

        /// <summary>
        /// Физические траектории движения ТС в заданноом интервале
        /// </summary>
        public VehicleTrajectory[] Trajectories { get; set; } = [];
    }
}
