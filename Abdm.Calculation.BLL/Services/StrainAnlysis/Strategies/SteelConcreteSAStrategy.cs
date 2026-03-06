using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis.Strategies
{
    public class SteelConcreteSAStrategy : ISAStrategy
    {
        public StrainCalculationGroupTypeEnum[] StrainCalculationGroupTypes { get => [
            StrainCalculationGroupTypeEnum.SteelConcrete
        ];}

        public AnalysisSummary Analyse(AnalysisSummary analysis, VehicleRollingResult vehicleRollingResult)
        {
            throw new NotImplementedException();
        }
    }
}
