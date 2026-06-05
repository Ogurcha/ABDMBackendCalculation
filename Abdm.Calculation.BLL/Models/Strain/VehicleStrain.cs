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
        /// Где на траетории находится ТС
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Где на траетории находится ТС
        /// </summary>
        public double Y { get; set; }

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
        public override double TotalStrain { get; set; }

        /// <summary>
        /// Напряжение по колёсам ТС
        /// </summary>
        public required WheelStrain[] WheelStrains { get; set; }

        /// <summary>
        /// Направление движения ТС, которое использовалось для расчёта напряжения
        /// true - ТС смотрит в направлении движения, false - ТС смотрит против направления движения
        /// </summary>
        public required bool IsDirectionForward { get; set; }

        /// <summary>
        /// Итоговое напряжение от ТС, если бы оно было расположено в противоположном направлении.
        /// Не влияет на алгоритмы сравнения, суммирования и т.д., 
        /// т.к. в <see cref="InvertedDirectionStrain"/> содержит напряжение заведомо меньшее, а, значит, не актуальное. 
        /// Используется только для аналитики и отображения в отчётах.
        /// </summary>
        public VehicleStrain? InvertedDirectionStrain { get; set; }

        /// <summary>
        /// Промежутки положительных кусков профиля, на которых было обнаружено данное напряжение
        /// </summary>
        public required Dictionary<ProfileYZ, HashSet<Interval>> PositivePiecesMap { get; set; }

        /// <summary>
        /// Рассчетная лямбда, которая считается как суммарная длина по всем <see cref="PositivePiecesMap"/>
        /// </summary>
        public double LambdaSmall { get; set; }
    }
}
