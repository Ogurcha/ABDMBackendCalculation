using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.BLL.Models.StrainAnalysis.SteelConcrete;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis.Strategies
{
    public class SteelConcreteAnalysisWriter : DefaultAnalysisWriter, IAnalysisWriter
    {
        public override StrainCalculationGroupTypeEnum[] StrainCalculationGroupTypes { get => [
            StrainCalculationGroupTypeEnum.SteelConcrete,
        ];}

        public override AnalysisSummary Analyse(AnalysisSummary analysis, VehicleRollingResult vehicleRollingResult, bool doNegativeNumbers)
        {
            var result = base.Analyse(analysis, vehicleRollingResult, doNegativeNumbers);
            result.SteelConcrete = GetSteelConcrete(result, vehicleRollingResult);
            return result;
        }

        private List<AnalysisSteelConcrete>? GetSteelConcrete(AnalysisSummary result, VehicleRollingResult vehicleRollingResult)
        {
            throw new NotImplementedException();
        }
    }
}
