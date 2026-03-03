using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IStrainAnalyser
    {
        AnalysisSummary GetAnalysis(VehicleRollingResult defaultRolling, VehicleRollingResult mirroredRolling);
    }
}