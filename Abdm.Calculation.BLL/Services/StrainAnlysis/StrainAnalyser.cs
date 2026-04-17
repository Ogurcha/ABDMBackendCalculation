using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.Maths.Extensions;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis
{
    public class StrainAnalyser(IStrainAnalyserFactory analyserFactory) : IStrainAnalyser
    {
        public AnalysisSummary? GetAnalysis(
            VehicleRollingResult defaultRoll,
            VehicleRollingResult mirroredRoll)
        {
            var hasDefault = defaultRoll.StrainResults.Any();
            var hasMirrored = mirroredRoll.StrainResults.Any();

            if (!hasDefault && !hasMirrored)
                return null;

            VehicleRollingResult maxStrainResult;
            if (!hasDefault)
            {
                maxStrainResult = mirroredRoll;
            }
            else if (!hasMirrored)
            {
                maxStrainResult = defaultRoll;
            }
            else
            {
                var defaultMax = defaultRoll.StrainResults.Max(x => x.Strain.TotalStrain);
                var mirroredMax = mirroredRoll.StrainResults.Max(x => x.Strain.TotalStrain);
                maxStrainResult = defaultMax >= mirroredMax ? defaultRoll : mirroredRoll;
            }

            var dataModel = defaultRoll.DataModel;

            var summary = new AnalysisSummary
            {
                CalculationType = dataModel.Data.Surface.StrainCalculationGroupType,
                AbsolutePositionLeft = MathExtensions.ToDecimal(dataModel.Intervals.First().AbsolutePositionLeft),
                AbsolutePositionRight = MathExtensions.ToDecimal(dataModel.Intervals.Last().AbsolutePositionRight),
                StrainCalculationGroupType = dataModel.Data.Surface.StrainCalculationGroupType
            };
            
            var analyser = analyserFactory.GetStrainAnalyser(summary.CalculationType);
            analyser.Analyse(summary, maxStrainResult);

            return summary;
        }
    }
}
