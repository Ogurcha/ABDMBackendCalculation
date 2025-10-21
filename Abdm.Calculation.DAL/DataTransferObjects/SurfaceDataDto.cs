using Abdm.Calculation.DAL.Enums;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.DAL.Entities
{
    /// <summary>
    /// Информация о поверхности влияния
    /// </summary>
    public class SurfaceDataDto
    {
        public bool? IsSymmetric { get; set; }

        public bool? IsGridRegular { get; set; }

        /// <summary>
        /// Число точек. Дублирует <see cref="Points"/>.Length, но нужно для чтения BinaryReader'ом
        /// </summary>
        public int PointsCount { get; set; }

        /// <summary>
        /// Точки поверхности влияния в 3д пространстве
        /// </summary>
        public required Vector3D[] Points { get; set; }

        /// <summary>
        /// Число треугольникув. Дублирует <see cref="Triangles"/>.Length, но нужно для чтения BinaryReader'ом
        /// </summary>
        public int TrianglesCount { get; set; }

        /// <summary>
        /// Полигоны. Индексы точек (base 0), по которым нужно соединять точки, чтобы получить пов-ть
        /// </summary>
        public Vector3I[]? Triangles { get; set; }

        /// <summary>
        /// тип проверки на чекпоинте - зависит от типа дефформации
        /// </summary>
        public StrainCalculationTypeEnum StrainCalculationType { get; set; }

        /// <summary>
        /// тип чекпоинта - балка или опора
        /// </summary>
        public CheckPointTypeEnum CheckPointType { get; set; }

        /// <summary>
        /// лямбда - используется для расчета коеффициентов напряжения
        /// </summary>
        public double Lambda { get; set; }
    }
}
