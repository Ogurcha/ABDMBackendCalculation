using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Models
{
    public class StrainScore
    {
        public double Score { get; set; }

        ///// <summary>
        ///// Скор при применения коэеффициентов полосности
        ///// </summary>
        //public double TotalScore { get; set; }

        public required List<StrainsInMaximums> StrainsPicked { get; set; }
    }
}
