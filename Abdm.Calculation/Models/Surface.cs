using System.Numerics;

namespace Abdm.Calculation.Models
{
    public class Surface
    {
        /// <summary>
        /// Массив точек, из которых состоит данная поверхность
        /// </summary>
        required public Vector3[] SurfacePoints { get; set; }

        public float MinZ { get; set; }

        /// <summary>
        /// Данные по опоре. Если чекпоинт не являтся опорой - массив пустой
        /// </summary>
        public float[] PillarData { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по X
        /// </summary>
        public float MaxX { get; set; }

        /// <summary>
        /// Минимальное значение всех точек по X
        /// </summary>
        public float MinX { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по Y
        /// </summary>
        public float MaxY { get; set; }

        /// <summary>
        /// Минимальное значение всех точек по Y
        /// </summary>
        public float MinY { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по Z
        /// </summary>
        public float MaxZ { get; set; }

        /// <summary>
        /// Перечисление, указывающее на то, как поверхность будет подвергаться нагрузке
        /// </summary>
        public CheckPointEnum CheckPointType { get; set; }

        /// <summary>
        /// Проектная устойчивость структуры. Без учёта собственного веса
        /// </summary>
        public float MyStrength { get; set; }

        /// <summary>
        /// Фиксированная нагрузка от собственного веса
        /// </summary>
        public float СonstLoad { get; set; }

        /// <summary>
        /// Нагрузка от пешеходов
        /// </summary>
        public float PedestrianLoad { get; set; } 

        /// <summary>
        /// Другая нагрузка
        /// </summary>
        public float OtherLoad { get; set; }

        /// <summary>
        /// Коэффициент устойчивости. По дефолту всегда 1.
        /// </summary>
        public float KStrength { get; set; }
    }
}
