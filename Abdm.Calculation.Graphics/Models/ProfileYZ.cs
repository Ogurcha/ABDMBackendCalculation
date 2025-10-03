using g4;

namespace Abdm.Calculation.Graphics.Models
{
    /// <summary>
    /// Кривая в плоскости YZ, обозначающая поверхность влияния в этой плоскости
    /// </summary>
    public class ProfileYZ
    {
        /// <summary>
        /// значение по оси X
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// точки, через которые проходит кривая, отсортированные по Y
        /// первые значения у всех точек идентичны <see cref="X"/>, но не
        /// стал маппить в 2д вектора, чтобы не было путаницы с наименованиями осей
        /// </summary>
        public required SortedList<double, Vector3d> Vectors { get; set; }
    }
}
