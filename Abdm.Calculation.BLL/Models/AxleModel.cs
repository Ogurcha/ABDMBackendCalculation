namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Информация об автомобильной оси
    /// </summary>
    public class AxleModel
    {
        /// <summary>
        /// Расстояние от оси и предыдущим объекта
        /// </summary>
        public double RelativePosition { get; set; }

        /// <summary>
        /// Абсолютная позиция оси, с учётом ВСЕХ объектов позади
        /// </summary>
        public double AbsolutePosition { get; set; }

        /// <summary>
        /// Вес оси
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Расстояние между колёсами. 
        /// Теоретически, расстояний может быть несколько, 
        /// и, соответветсвенно, колёс в оси больше двух
        /// </summary>
        public double[]? WheelsDistance { get; set; }

        public double Wx { get; set; }

        public double Wy { get; set; }

        public double WheelWeight => Weight / WheelCount;

        public int WheelCount => (WheelsDistance?.Length ?? 0) + 1;
    }
}
