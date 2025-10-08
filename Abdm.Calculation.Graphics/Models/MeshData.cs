namespace Abdm.Calculation.Graphics.Models
{
    /// <summary>
    /// Сетка поверхности влияния имеет "квадратную" структуру, при которой определенные значения X и Y повторяются очень часто.
    /// Поэтому имеет смысл в оптимизационых целях использовать <see cref="DistinctXs"/> и <see cref="DistinctYs"/> вместо прямого перебора всех точек
    /// </summary>
    public class MeshData
    {
        /// <summary>
        /// Уникальные значения точек по оси Х.
        /// </summary>
        public double[]? DistinctXs { get; set; }

        /// <summary>
        /// Уникальные значения точек по оси Y.
        /// </summary>
        public double[]? DistinctYs { get; set; }

        /// <summary>
        /// Закешированные профили срезов плоскостями YZ
        /// </summary>
        public List<ProfileYZ> Profiles { get; set; } = [];
    }
}
