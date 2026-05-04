namespace Abdm.Calculation.BLL.Models
{
    /// <summary>
    /// Интервал
    /// </summary>
    public class Interval
    {
        /// <summary>
        /// Начало инетервала
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        /// Конец интервала
        /// </summary>
        public double End { get; set; }

        private double? _length;
        public double Length
        {
            get
            {
                _length ??= End - Start;
                return _length.Value;
            }
        }
    }
}
