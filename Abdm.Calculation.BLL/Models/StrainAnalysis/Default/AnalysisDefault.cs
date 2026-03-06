namespace Abdm.Calculation.BLL.Models.StrainAnalysis.Default
{
    public class AnalysisDefault()
    {
        public bool HasSafetyLine { get; set; }

        public required AnalysisVehicle[] Vehicles { get; set; }
    }
}
