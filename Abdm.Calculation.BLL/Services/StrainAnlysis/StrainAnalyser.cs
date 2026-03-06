using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.Maths.Extensions;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis
{
    public class StrainAnalyser(IStrainAnalyserFactory analyserFactory) : IStrainAnalyser
    {
        public AnalysisSummary GetAnalysis(
            VehicleRollingResult defaultRoll,
            VehicleRollingResult mirroredRoll)
        {
            var maxStrainResult = defaultRoll.StrainResults.OrderBy(x => x.Strain.TotalStrain).Last().Strain.TotalStrain >= mirroredRoll.StrainResults.OrderBy(x => x.Strain.TotalStrain).Last().Strain.TotalStrain ? defaultRoll : mirroredRoll;
            var dataModel = defaultRoll.DataModel;

            var summary = new AnalysisSummary {
                CalculationType = dataModel.Data.Surface.StrainCalculationGroupType,
                AbsolutePositionLeft = MathExtensions.ToDecimal(dataModel.Intervals.First().AbsolutePositionLeft),
                AbsolutePositionRight = MathExtensions.ToDecimal(dataModel.Intervals.Last().AbsolutePositionRight)
            };

            var analyser = analyserFactory.GetStrainAnalyser(summary.CalculationType);
            analyser.Analyse(summary, maxStrainResult);

            return summary;
        }
    }
}
