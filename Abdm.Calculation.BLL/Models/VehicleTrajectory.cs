namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Траектория движения транспортного средства
    /// </summary>
    public class VehicleTrajectory
    {
        /// <summary>
        /// Траектория колёс слева. 
        /// Ключ - <see cref="Axle.WheelsDistance"/>
        /// </summary>
        public required SortedList<double, ProfileYZ> Left { get; set; }

        /// <summary>
        /// траектория условного центра
        /// </summary>
        public required ProfileYZ Center { get; set; }

        /// <summary>
        /// Траектория колёс слева. 
        /// Ключ - <see cref="Axle.WheelsDistance"/>
        /// </summary>
        public required SortedList<double, ProfileYZ> Right { get; set; }

        /// <summary>
        /// Координата X условного центра траектории
        /// </summary>
        public double X => Center.X;

        /// <summary>
        /// Суперпрофиль, который содержит в себе суммы всех профилей данной траектории. 
        /// Суперпрофиля может быть два в случае двунаправленного движения. 
        /// </summary>
        public Dictionary<bool, ProfileYZ>? SuperProfile { get; set; }
    }
}
