namespace Abdm.Calculation.SteelConcrete.Models
{
    public class IssoSteelConcreteParameters
    {
        public double Ea { get; internal set; }

        public double Es { get; internal set; }

        public double Eb { get; internal set; }

        public double M1 { get; internal set; }

        public double M2g { get; internal set; }

        public double Mp { get; internal set; }

        /// <summary>
        /// Расчетное сопротивление бетона на сжатие, МПа
        /// </summary>
        public double Rb { get; internal set; }

        /// <summary>
        /// Расчетное сопротивление арматуры, МПа
        /// </summary>
        public double Rr { get; internal set; }

        public double Sd { get; internal set; }

        public double L { get; internal set; }

        /// <summary>
        /// Расчетное сопротивление стали нижнего пояса, МПа
        /// </summary>
        public double Rs1 { get; internal set; }

        /// <summary>
        /// Расчетное сопротивление стали верхнего пояса, МПа
        /// </summary>
        public double Rs2 { get; internal set; }

        /// <summary>
        /// Предельная деформация бетона на сжатие
        /// </summary>
        public double EpsilonBetaLim { get; internal set; }

        /// <summary>
        /// Коэффициент пластических деформаций стальных поясов
        /// </summary>
        public double X1Coefficient { get; internal set; }

        public bool IsNegative { get; internal set; }

        public PlateTypeEnum PlateType { get; internal set; }

        /// <summary>
        /// Предельная характеристика ползучести бетона
        /// </summary>
        public double? TetaKrParam { get; internal set; }

        /// <summary>
        /// Напряжения в бетоне от усадки σ(b,shr), МПа
        /// </summary>
        public double? SigmaBetaShrParam { get; internal set; }

        /// <summary>
        /// апряжения в арматуре от усадки σ(a,shr), МПа
        /// </summary>
        public double? SigmaAlfaShrParam { get; internal set; }

        /// <summary>
        /// Напряжения в бетоне от разницы температур σ(b,t), МПа
        /// </summary>
        public double? SigmaBetaTParam { get; internal set; }

        /// <summary>
        /// Напряжения в арматуре от разницы температур σ(a,t), МПа
        /// </summary>
        public double? SigmaAlfaTParam { get; internal set; }

        /// <summary>
        /// Максимальная разность температур, С˚
        /// </summary>
        public double TMax { get; internal set; }

    }
}
