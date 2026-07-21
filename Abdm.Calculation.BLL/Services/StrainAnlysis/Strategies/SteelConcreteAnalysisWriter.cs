using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.BLL.Models.StrainAnalysis.SteelConcrete;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis.Strategies
{
    public class SteelConcreteAnalysisWriter(ISteelConcreteOriginFacade steelConcreteOriginFacade) : DefaultAnalysisWriter, IAnalysisWriter
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
            
        private AnalysisSteelConcrete? GetSteelConcrete(AnalysisSummary result, VehicleRollingResult vehicleRollingResult)
        {
            if (vehicleRollingResult.DataModel.Data.Surface.StrainTypeSpecificData is not SteelConcreteData steelConcreteData
                || vehicleRollingResult.StrainResults.MaxBy(x => x.TotalStrain) is not StrainResult strainResult)
            {
                return null;
            }

            return steelConcreteOriginFacade.Analyse(strainResult, steelConcreteData);
        }
    }
}
