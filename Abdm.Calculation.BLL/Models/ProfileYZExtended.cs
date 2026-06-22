namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Расширенный профиль <see cref="ProfileYZ"/> для случаев, когда необходимо считать объёмы поверхности влияния под полосой
    /// </summary>
    public class ProfileYZExtended : ProfileYZ
    {
        /// <summary>
        /// Профили между точками под центром колеса и под кромкой колеса слева
        /// </summary>
        public required Dictionary<Axle, ProfileYZBase[]> VolumetricProfiles { get; set; }

        /// <summary>
        /// Длина отпечатка колеса
        /// </summary>
        public required Dictionary<Axle, double> FootprintLength { get; set; }

        /// <summary>
        /// Ширина отпечатка колеса
        /// </summary>
        public required Dictionary<Axle, double> FootprintWidth { get; set; }
    }
}
