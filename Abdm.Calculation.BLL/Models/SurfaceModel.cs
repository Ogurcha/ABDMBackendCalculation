using System.Text.Json.Serialization;

namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Модель поверхности, по которой едет ТС
    /// </summary>
    public class SurfaceModel
    {
        /// <summary>
        /// Максимальное значение всех точек по Y
        /// </summary>
        public double MaxY { get; set; }

        /// <summary>
        /// Минимальное значение всех точек по Y
        /// </summary>
        public double MinY { get; set; }

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
        /// лямбда - используется для расчета коеффициентов напряжения
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>
        /// перевёрнута ли поверхность
        /// </summary>
        public bool IsMirroredByZ { get; internal set; } = false;
    }
}
