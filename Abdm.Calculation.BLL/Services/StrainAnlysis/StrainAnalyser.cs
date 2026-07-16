using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Default;
using Abdm.Calculation.Maths.Extensions;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis
{
    public class StrainAnalyser(IStrainAnalyserFactory analyserFactory) : IStrainAnalyser
    {
        public AnalysisSummary? GetAnalysis(
            VehicleRollingResult defaultRoll,
            VehicleRollingResult? mirroredRoll)
        {
            var hasDefault = defaultRoll.StrainResults.Length > 0;
            var hasMirrored = mirroredRoll?.StrainResults?.Length > 0;
            var doNegativeNumbers = false;

            if (!hasDefault && !hasMirrored)
                return null;

            VehicleRollingResult maxStrainResult;
            if (!hasDefault)
            {
                maxStrainResult = mirroredRoll!;
            }
            else if (!hasMirrored)
            {
                maxStrainResult = defaultRoll;
            }
            else
            {
                var defaultMax = defaultRoll.StrainResults.Max(x => x.TotalStrain);
                var mirroredMax = mirroredRoll!.StrainResults.Max(x => x.TotalStrain);
                maxStrainResult = defaultMax >= mirroredMax ? defaultRoll : mirroredRoll;
                if (maxStrainResult == mirroredRoll)
                {
                    doNegativeNumbers = true;
                }
            }

            var dataModel = defaultRoll.DataModel;

            var summary = new AnalysisSummary
            {
                StrainCalculationGroupType = dataModel.Data.Surface.StrainCalculationGroupType,
                BarrierInfo = GetBarrierInfo(dataModel),
                StrainCalculationType = dataModel.Data.Surface.StrainCalculationType,
            };
            
            var analyser = analyserFactory.GetStrainAnalyser(summary.StrainCalculationGroupType);
            analyser.Analyse(summary, maxStrainResult, doNegativeNumbers);

            return summary;
        }

        private BarrierInfo GetBarrierInfo(VehicleRollingBigModel dataModel)
        {
            bool hasBarrierInTheMiddle = dataModel.Intervals.Any(x => x.Type != Enums.PassageIntervalTypeEnum.WholeInterval);

            var absolutePositionFarLeft = MathExtensions.ToDecimal(dataModel.Intervals.First().AbsolutePositionLeft);
            decimal? absolutePositionMiddleLeft = hasBarrierInTheMiddle 
                ? MathExtensions.ToDecimal(dataModel.Intervals.First().AbsolutePositionRight)
                : null;
            decimal? absolutePositionMiddleRight = hasBarrierInTheMiddle
                ? MathExtensions.ToDecimal(dataModel.Intervals.Last().AbsolutePositionLeft)
                : null;
            var absolutePositionFarRight = MathExtensions.ToDecimal(dataModel.Intervals.Last().AbsolutePositionRight);

            var shift = absolutePositionFarLeft;

            return new BarrierInfo
            {
                HasBarrierInTheMiddle = hasBarrierInTheMiddle,
                AbsolutePositionFarLeft = absolutePositionFarLeft,
                AbsolutePositionMiddleLeft = absolutePositionMiddleLeft,
                AbsolutePositionMiddleRight = absolutePositionMiddleRight,
                AbsolutePositionFarRight = absolutePositionFarRight,
                PositionFarLeft = 0,
                PositionMiddleLeft = absolutePositionMiddleLeft - shift,
                PositionMiddleRight = absolutePositionMiddleRight - shift,
                PositionFarRight = absolutePositionFarRight - shift,
            };
        }
    }
}
