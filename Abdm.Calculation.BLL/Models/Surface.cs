using System.Text.Json.Serialization;
using Abdm.Calculation.DAL.Enums;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Models
{
    public class Surface
    {
        /// <summary>
        /// Массив точек, из которых состоит поверхность влияния
        /// </summary>
        required public Vector3D[] SurfacePoints { get; set; }

        /// <summary>
        /// Данные по опоре. Если чекпоинт не являтся опорой - массив пустой
        /// </summary>
        public required Vector2D[] PillarData { get; set; }

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
        public CheckPointTypeEnum CheckPointType { get; set; }

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

        #region SteelConcreteProperties
        public double? WorkSign { get; set; }

        public double? Es { get; set; }

        public double? Ea { get; set; }

        public double? Eb { get; set; }

        public double? TetaKr { get; set; }

        public double? EpsilonBetaLim { get; set; }

        public double? Rs1 { get; set; }

        public double? Rs2 { get; set; }

        public double? Rr { get; set; }

        public double? Rb { get; set; }

        public double? Tmax { get; set; }

        public double? PlateType { get; set; }

        public double? L { get; set; }

        public double? Sd { get; set; }

        public double? SigmaBetaKr { get; set; }

        public double? SigmaAlfaKr { get; set; }

        public double? SigmaBetaShr { get; set; }

        public double? SigmaAlfaShr { get; set; }

        public double? SigmaBetaT { get; set; }

        public double? SigmaAlfaT { get; set; }

        public double? M1 { get; set; }

        public double? M2g { get; set; }

        public double? Xsi1 { get; set; }

        public double? Mp { get; set; }
        #endregion
    }
}
