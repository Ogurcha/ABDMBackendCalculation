namespace Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete
{
    public class PermissibleLoadClasses
    {
        /// <summary>
        /// Допустимые классы эталонных временных нагрузок
        /// Permissible classes of reference (temporary) loads (descriptive string or code).
        /// </summary>
        public required string PermissibleReferenceTemporaryLoadClasses { get; set; }

        /// <summary>
        /// Изгибающие моменты от временных нагрузок, МН*м
        /// Bending moments from temporary loads, MN*m.
        /// </summary>
        public decimal TemporaryLoadsMoments { get; set; }

        /// <summary>
        /// Полный изгибающий момент на второй стадии, МН*м
        /// Full bending moment at the second stage, MN*m.
        /// </summary>
        public decimal FullBendingMomentSecondStage { get; set; }

        /// <summary>
        /// Полный изгибающий момент, МН*м
        /// Full bending moment (resulting total), MN*m.
        /// </summary>
        public decimal FullBendingMoment { get; set; }
    }
}
