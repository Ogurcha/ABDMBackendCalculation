using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis
{
    public class StrainAnalyser(IStrainAnalyserFactory analyserFactory) : IStrainAnalyser
    {
        public AnalysisSummary GetAnalysis(
            VehicleRollingResult defaultRoll,
            VehicleRollingResult mirroredRoll)
        {
            var strains = defaultRoll.StrainResults.Union(mirroredRoll.StrainResults);
            var maxStrainResult = strains.OrderBy(x => x.Strain.TotalStrain).Last();
            var dataModel = defaultRoll.DataModel;

            var summary = new AnalysisSummary {
                CalculationType = dataModel.Data.Surface.StrainCalculationGroupType,
                RoadRule = maxStrainResult.RoadRuleRef
            };

            var analyser = analyserFactory.GetStrainAnalyser(summary.CalculationType);
            analyser.Analyse(summary, maxStrainResult, dataModel);

            return summary;
        }

        

        
    }
}
