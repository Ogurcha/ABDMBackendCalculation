namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Pillar
{
    public class TrafficJamStrainAnalysisSlim
    {
        public int Number { get; set; }

        public decimal IntervalStart { get; set; }

        public decimal IntervalEnd { get; set; }

        public decimal IntervalLength { get; set; }

        public decimal SumStrain { get; set; }
    }
}
