using g4;

namespace Abdm.Calculation.Graphics
{
    public class Mesh
    {
        public DMeshAABBTree3 Tree { get; set; }

        /// <summary>
        /// Вычисляемые данные по кэшу. Здесь происходит денормализация и дублирование во имя оптимизации
        /// </summary>
        public MeshData Data { get; set; }
    }

    /// <summary>
    /// Вычисляемые данные по кэшу. Здесь происходит денормализация и дублирование во имя оптимизации
    /// </summary>
    public class MeshData
    {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MinZ { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double MaxZ { get; set; }

        /// <summary>
        /// Уникальные значения точек по оси Х.
        /// </summary>
        public double[] DistinctXs { get; set; }

        /// <summary>
        /// Уникальные значения точек по оси Х + с учётом размера колёс
        /// </summary>
        public double[] DistinctXsWithWheels { get; set; }
    }
}
