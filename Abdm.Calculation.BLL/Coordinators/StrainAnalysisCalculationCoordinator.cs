using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Mapster;

namespace Abdm.Calculation.BLL.Coordinators
{
    public class StrainAnalysisCalculationCoordinator(
        IBaseVehicleRollingCalculationCoordinator baseCoordinator,
        IStrainAnalyser strainAnalyser
        ) : ICoordinator<StrainAnalysisParameters, StrainAnalysisResult>
    {
        public async Task<ResultMonad<StrainAnalysisResult>> Run(
            [DisallowNull] StrainAnalysisParameters parameters,
            CancellationToken cancellationToken)
        {
            var bigDataResult = await baseCoordinator.PrepareDataModel(parameters.Adapt<PassTypeCalculationParameters>(), null, cancellationToken);
            if (!bigDataResult.IsSuccess)
            {
                return new ResultMonad<StrainAnalysisResult>(bigDataResult.Exception!);
            }
            var data = bigDataResult.Result!;

            var defaultRollResult = GetRollingResult(data, cancellationToken, out VehicleRollingResult? backwardRollingResult);
            if (!defaultRollResult.IsSuccess)
            {
                return new ResultMonad<StrainAnalysisResult>(defaultRollResult.Exception!);

            }
            var defaultRoll = defaultRollResult.Result!;

            data.FlipMeshes();
            var mirroredRollResult = GetRollingResult(data, cancellationToken, out VehicleRollingResult? mirroredBackwardRollingResult);
            if (!mirroredRollResult.IsSuccess)
            {
                return new ResultMonad<StrainAnalysisResult>(mirroredRollResult.Exception!);
            }
            var mirroredRoll = mirroredRollResult.Result!;

            var strainAnalysis = strainAnalyser.GetAnalysis(defaultRoll, mirroredRoll, backwardRollingResult, mirroredBackwardRollingResult);
            if (strainAnalysis == null)
            {
                return new ResultMonad<StrainAnalysisResult>(GetFailedResult(parameters));
            }

            return new ResultMonad<StrainAnalysisResult>(ComposeMessage(parameters, strainAnalysis, data.TrianglesToCache));
        }

        /// <summary>
        /// HACK: Двунаправленное движение в отчётах должно работать нелогично
        /// При двунаправленном, сначала ВСЕ ТС должны смотреть прямо, потом ВСЕ назад
        /// Вразнобой нельзя ибо так написано в нормах, несмотря на то, что вразнобой можно найти более невыгодное положение тележек
        /// </summary>
        private ResultMonad<VehicleRollingResult> GetRollingResult(
            VehicleRollingBigModel data, 
            CancellationToken cancellationToken, 
            out VehicleRollingResult? backwardRollingResult)
        {
            backwardRollingResult = null;
            if (data.Data.Load.ActualDirection.Length > 1)
            {
                data.Data.Load.ActualDirection = [true];
                var directionForwardResult = baseCoordinator.RollAndGetStrainResult(data, cancellationToken);
                if (!directionForwardResult.IsSuccess)
                {
                    return directionForwardResult;
                }
                data.Data.Load.ActualDirection = [false];
                var directionBackwardResult = baseCoordinator.RollAndGetStrainResult(data, cancellationToken);
                if (!directionBackwardResult.IsSuccess)
                {
                    return directionBackwardResult;
                }
                data.Data.Load.ActualDirection = [true, false];
                backwardRollingResult = directionBackwardResult.Result;
                return directionForwardResult;
            }
            else
            {
                return baseCoordinator.RollAndGetStrainResult(data, cancellationToken);
            }
        }

        public string InfoMsg(StrainAnalysisParameters param)
        {
            return string.Format("Strain analysis for (IssoId = {0}, Check point number = {1}) started", param.IssoId, param.CheckPointNumber);
        }

        public string ErrorMsg(StrainAnalysisParameters param)
        {
            return string.Format("Error while making strain analysis for {0}, n = {1}", param.IssoId, param.CheckPointNumber);
        }

        public string ExceptionMsg(StrainAnalysisParameters param)
        {
            return string.Format("Failed strain analysis for (IssoId = {0}, Check point number = {1})", param.IssoId, param.CheckPointNumber);
        }

        public StrainAnalysisResult GetFailedResult(StrainAnalysisParameters param)
        {
            return new StrainAnalysisResult()
            {
                IssoId = param.IssoId,
                CheckPointNumber = param.CheckPointNumber,
                LoadId = (int)param.LoadSchema.Id,
                Direction = (int)param.Direction,
                SnipId = (int)param.Snip,
                Data = new AnalysisSummary()
                {
                    StrainCalculationGroupType = Enums.StrainCalculationGroupTypeEnum.Unknown,
                    BarrierInfo = new Models.StrainAnalysis.Default.BarrierInfo()
                },
                ReportId = param.ReportId,
            };
        }

        private StrainAnalysisResult ComposeMessage(StrainAnalysisParameters param, AnalysisSummary analysisSummary, Maths.Models.Vector3I[]? trianglesToCache)
        {
            return new StrainAnalysisResult()
            {
                IssoId = param.IssoId,
                CheckPointNumber = param.CheckPointNumber,
                LoadId = (int)param.LoadSchema.Id,
                Direction = (int)param.Direction,
                SnipId = (int)param.Snip,
                Data = analysisSummary,
                ReportId = param.ReportId,
                TrianglesToCache = trianglesToCache
            };
        }
    }
}
