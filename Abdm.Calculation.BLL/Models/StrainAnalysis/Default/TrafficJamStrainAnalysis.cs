namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class TrafficJamStrainAnalysis
    {
        public int Number { get; set; }

        public decimal LeftIntervalStart { get; set; }

        public decimal LeftIntervalEnd { get; set; }

        public decimal LeftIntervalLength { get; set; }

        public decimal LeftIntervalStrain { get; set; }

        public decimal RightIntervalStart { get; set; }

        public decimal RightIntervalEnd { get; set; }

        public decimal RightIntervalLength { get; set; }

        public decimal RightIntervalStrain { get; set; }

        public decimal SumStrain { get; set; }
    }
}
