using System.Numerics;
using Abdm.Calculation.BLL.Enums;

namespace Abdm.Calculation.BLL.Models
{
    public class Surface
    {
        /// <summary>
        /// Массив точек, из которых состоит поверхность влияния
        /// </summary>
        required public SurfacePoint[] SurfacePoints { get; set; }

        /// <summary>
        /// Данные по опоре. Если чекпоинт не являтся опорой - массив пустой
        /// </summary>
        public required double[] PillarData { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по X
        /// </summary>
        public double MaxX { get; set; }

        /// <summary>
        /// Минимальное значение всех точек по X
        /// </summary>
        public double MinX { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по Y
        /// </summary>
        public double MaxY { get; set; }

        /// <summary>
        /// Минимальное значение всех точек по Y
        /// </summary>
        public double MinY { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по Z
        /// </summary>
        public double MaxZ { get; set; }

        /// <summary>
        /// Перечисление, указывающее на то, как поверхность будет подвергаться нагрузке
        /// CpSubType в старом клиенте
        /// </summary>
        public CheckPointEnum CheckPointType { get; set; }

        /// <summary>
        /// Проектная устойчивость структуры. Без учёта собственного веса
        /// </summary>
        public double MyStrength { get; set; }

        /// <summary>
        /// Фиксированная нагрузка от собственного веса
        /// </summary>
        public double ConstLoad { get; set; }

        /// <summary>
        /// Нагрузка от пешеходов
        /// </summary>
        public double PedestrianLoad { get; set; } 

        /// <summary>
        /// Другая нагрузка
        /// </summary>
        public double OtherLoad { get; set; }

        /// <summary>
        /// Коэффициент устойчивости. По дефолту всегда 1.
        /// </summary>
        public double KStrength { get; set; }
    }
}
