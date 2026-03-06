using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISAStrategy
    {
        public StrainCalculationGroupTypeEnum[] StrainCalculationGroupTypes { get; }

        public AnalysisSummary Analyse(AnalysisSummary analysis, VehicleRollingResult vehicleRollingResult);
    }
}
  