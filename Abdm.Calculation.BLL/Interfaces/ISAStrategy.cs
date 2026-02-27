using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISAStrategy
    {
        public StrainCalculationGroupTypeEnum[] StrainCalculationGroupTypes { get; }

        public AnalysisSummary Analyse(AnalysisSummary analysis, StrainResult strainResult, VehicleRollingBigModel bigModel);
    }
}
  