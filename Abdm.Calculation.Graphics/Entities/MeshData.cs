namespace Abdm.Calculation.Graphics.Entities
{
    /// <summary>
    /// Вычисляемые данные по кэшу. Здесь происходит денормализация и дублирование во имя оптимизации
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
        /// Уникальные значения точек по оси Х + с учётом размера колёс
        /// </summary>
        public double[]? DistinctXsWithWheels { get; set; }
    }
}
