using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Расширенный профиль <see cref="ProfileYZ"/> для случаев, когда необходимо считать объёмы поверхности влияния под полосой
    /// </summary>
    public class ProfileYZExtended : ProfileYZ
    {
        /// <summary>
        /// Профиль под кромкой колеса слева
        /// </summary>
        public required Vector2D[] SortedVectorsLeft { get; set; }

        /// <summary>
        /// Профиль под кромкой колеса справа
        /// </summary>
        public required Vector2D[] SortedVectorsRight { get; set; }

        /// <summary>
        /// Длина отпечатка колеса
        /// </summary>
        public required double FootprintLength { get; set; }

        /// <summary>
        /// Ширина отпечатка колеса
        /// </summary>
        public required double FootprintWidth { get; set; }
    }
}
