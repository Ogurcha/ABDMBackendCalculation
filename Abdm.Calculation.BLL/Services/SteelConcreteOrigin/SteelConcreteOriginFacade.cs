using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis.SteelConcrete;
using AisPcCore.CheckPoint;
using AisPcCore.SfData;

namespace Abdm.Calculation.BLL.Services.SteelConcreteOrigin
{
    public class SteelConcreteOriginFacade : ISteelConcreteOriginFacade
    {
        public AnalysisSteelConcrete Analyse(StrainResult strainResult, SteelConcreteData steelConcreteData)
        {
            ArgumentNullException.ThrowIfNull(strainResult);
            ArgumentNullException.ThrowIfNull(steelConcreteData);

            if (steelConcreteData.SteelConcreteParameters is not { } parameters)
            {
                throw new InvalidOperationException("Steel concrete parameters are required for analysis.");
            }

            var checkPoint = SteelConcreteOriginMapper.CreateCheckPoint(steelConcreteData, parameters);

            var values = new Dictionary<ais7SfUse, double>
            {
                [ais7SfUse.Single] = strainResult.TotalStrain,
            };

            checkPoint.GetStGbControlValue(values, ais7PassTypeEnum.NoLimit);

            var repVal = checkPoint.FirstCaseValues?.ReportValues
                ?? new aisReportValues_StGb();

            return SteelConcreteOriginMapper.MapToAnalysisSteelConcrete(
                checkPoint,
                repVal,
                parameters);
        }
    }
}
