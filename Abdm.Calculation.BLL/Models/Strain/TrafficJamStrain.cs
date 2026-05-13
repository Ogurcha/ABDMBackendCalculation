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
        public double Coefficient { get; set; } = 1d;

        /// <summary>
        /// Итоговое напряжение с учётом коэффициента
        /// </summary>
        public override double TotalStrain { get; set; }

        /// <summary>
        /// Напряжение от профилей слева от центра ТС
        /// </summary>
        public double LeftStrain { get; set; }

        /// <summary>
        /// Напряжение от профилей справа от центра ТС
        /// </summary>
        public double RightStrain { get; set; }
    }
}
