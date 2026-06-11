namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class Coefficients
    {
        public decimal Stripe { get; set; }

        public decimal Dynamic { get; set; }

        public decimal Reliability { get; set; }

        public decimal? StripeInterval { get; set; }

        public decimal? DynamicInterval { get; set; }

        public decimal? ReliabilityInterval { get; set; }
    }
}
