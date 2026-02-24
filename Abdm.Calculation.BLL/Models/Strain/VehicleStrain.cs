namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// Итоговое напряжение выдаваемое одним авто 
    /// в определённых координатах 
    /// на определённом сооружении
    /// </summary>
    public class VehicleStrain : ComparableStrainBase
    {
        /// <summary>
        /// Суммарное напряжение по всем осям одного ТС
        /// </summary>
        public double SumStrain { get; set; }

        /// <summary>
        /// Повышающий коэффициент напряжения
        /// </summary>
        public double Coefficient { get; set; } = 1d;

        /// <summary>
        /// Итоговое напряжение с учётом коэффициента
        /// </summary>
        public override double TotalStrain => SumStrain * Coefficient + (TrafficJamStrain?.TotalStrain ?? 0d);

        /// <summary>
        /// Напряжение по колёсам ТС
        /// </summary>
        public required WheelStrain[] WheelStrains { get; set; }

        /// <summary>
        /// Напряжение, которое эмулирует равномерное скопление машин в пробке
        /// </summary>
        public TrafficJamStrain? TrafficJamStrain { get; set; }
    }
}
