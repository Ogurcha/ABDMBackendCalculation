namespace Abdm.Calculation.BLL.Models.Strain
{
    /// <summary>
    /// один положительный участок полосовой нагрузки
    /// </summary>
    public class TrafficJamStrainPiece
    {   
        /// <summary>
        /// Интервал по длине, где находится участок полосовой нагрузки
        /// </summary>
        public required Interval Interval { get; set; }  

        /// <summary>
        /// Напряжение от профилей слева от центра ТС
        /// </summary>
        public double LeftStrain { get; set; }

        /// <summary>
        /// Объем пов-ти влияния слева от центра ТС
        /// </summary>
        public double LeftVolume { get; set; }

        /// <summary>
        /// Напряжение от профилей справа от центра ТС
        /// </summary>
        public double RightStrain { get; set; }

        /// <summary>
        /// Объем пов-ти влияния справа от центра ТС
        /// </summary>
        public double RightVolume { get; set; }
    }
}
