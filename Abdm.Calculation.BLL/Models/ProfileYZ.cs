using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Кривая в плоскости YZ, обозначающая поверхность влияния в этой плоскости
    /// </summary>
    public class ProfileYZ
    {
        /// <summary>
        /// значение по оси X - индентификатор профиля
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// точки, через которые проходит кривая, отсортированные по Y
        /// </summary>
        public required SortedList<double, Vector2D> Vectors { get; set; }

        /// <summary>
        /// Экстремумы кривой. 
        /// Концы кривой не включены в экстремумы, но на краях могут быть лишние экстремумы из-за зануления профиля по краям. 
        /// </summary>
        public required ProfileExtremum[] Extremums { get; set; }
    }
}
