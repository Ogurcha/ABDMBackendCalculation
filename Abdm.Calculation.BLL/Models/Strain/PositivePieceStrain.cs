namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// Напряжение типа <see cref="TrafficJamStrain"/> ТС
    /// На каком отрезке траектории движения
    /// </summary>
    public class PositivePieceStrain
    {
        /// <summary>
        /// Значение напряжения
        /// </summary>
        //public double Strain { get; set; }

        /// <summary>
        /// Расстояние от начала траектории движения
        /// до начала учёта данного напряжения
        /// </summary>
        public double BeginY { get; set; }

        /// <summary>
        /// Расстояние от начала траектории движения
        /// до конца учёта данного напряжения
        /// </summary>
        public double EndY { get; set; }
    }
}
