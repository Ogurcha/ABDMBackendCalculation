namespace Abdm.Calculation.DAL.Entities
{
    /// <summary>
    /// Информация о поверхности влияния
    /// </summary>
    public class SurfaceData
    {
        public bool IsSymmetric { get; set; }

        public bool IsGridRegular { get; set; }

        /// <summary>
        /// Число точек. Дублирует <see cref="Points"/>.Length, но нужно для чтения BinaryReader'ом
        /// </summary>
        public int PointsCount { get; set; }

        /// <summary>
        /// Точки поверхности влияния в 3д пространстве
        /// </summary>
        public required (double X, double Y, double Z)[] Points { get; set; }

        /// <summary>
        /// Число треугольникув. Дублирует <see cref="Triangles"/>.Length, но нужно для чтения BinaryReader'ом
        /// </summary>
        public int TrianglesCount { get; set; }

        /// <summary>
        /// Полигоны. Индексы точек (base 0), по которым нужно соединять точки, чтобы получить пов-ть
        /// </summary>
        public (int, int, int)[]? Triangles { get; set; }
    }
}
