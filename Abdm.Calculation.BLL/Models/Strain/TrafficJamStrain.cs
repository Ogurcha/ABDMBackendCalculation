namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// Напряжение, которое эмулирует равномерное скопление машин в пробке
    /// </summary>
    public class TrafficJamStrain : ComparableStrainBase
    {
        /// <summary>
        /// Суммарное напряжение данного типа
        /// </summary>
        public double SumStrain { get; set; }

        /// <summary>
        /// Повышающий коэффициент напряжения
        /// </summary>
        public double ReliabilityCoefficient { get; set; } = 1d;

        /// <summary>
        /// Итоговое напряжение с учётом коэффициента
        /// </summary>
        public override double TotalStrain { get; set; }

        /// <summary>
        /// Положительные участки полосовой нагрузки
        /// </summary>
        public required List<TrafficJamStrainPiece> StrainPieces { get; set; }
    }
}
