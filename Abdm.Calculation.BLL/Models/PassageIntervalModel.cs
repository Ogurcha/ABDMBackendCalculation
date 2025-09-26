namespace Abdm.Calculation.BLL.Entities
{
    public class PassageIntervalModel
    {
        public double TotalWidth { get; set; }

        public double SafetyLineLeft { get; set; }

        public double SafetyLineRight { get; set; }

        public double[] SafeInterval { get; set; } = new double[2];
    }
}
