namespace Abdm.Calculation.DAL.Entities
{
    public class PassageInterval
    {
        public double TotalWidth { get; set; }

        public double SafetyLineLeft { get; set; }

        public double SafetyLineRight { get; set; }

        public double[]? SafeInterval { get; set; }
    }
}
