namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Информация об автомобильной оси
    /// </summary>
    public class Axle
    {
        /// <summary>
        /// Расстояние относительной предыдущей тележки ТС
        /// </summary>
        public double RelativePosition { get; set; }

        /// <summary>
        /// Расстояние относительно начала ТС
        /// </summary>
        public double Position { get; set; }

        /// <summary>
        /// Вес оси
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Расстояния от центра оси ТС до колёс
        /// Теоретически, расстояний может быть несколько, 
        /// и, соответветсвенно, колёс в оси больше двух
        /// </summary>
        public required double[] WheelsDistance { get; set; }

        /// <summary>
        /// ширина проекции одного колеса
        /// </summary>
        public double WheelWidth { get; set; }

        /// <summary>
        /// длина проекции одного колеса
        /// </summary>
        public double WheelLength { get; set; }

        public double wheelWeight => Weight / WheelCount;

        public int WheelCount => WheelsDistance.Length * 2;
    }
}
