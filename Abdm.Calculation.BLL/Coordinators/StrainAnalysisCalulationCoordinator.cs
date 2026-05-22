using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Mapster;

namespace Abdm.Calculation.BLL.Coordinators
{
    public class StrainAnalysisCalulationCoordinator(
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

            var defaultRollResult = baseCoordinator.RollAndGetStrainResult(data, cancellationToken);
            if (!defaultRollResult.IsSuccess)
            {
                return new ResultMonad<StrainAnalysisResult>(defaultRollResult.Exception!);
            }
            var defaultRoll = defaultRollResult.Result!;
            data.FlipMeshes();
            var mirroredRollResult = baseCoordinator.RollAndGetStrainResult(data, cancellationToken);

            var strainAnalysis = strainAnalyser.GetAnalysis(defaultRoll, mirroredRollResult.Result);
            if (strainAnalysis == null)
            {
                return new ResultMonad<StrainAnalysisResult>(GetFailedResult(parameters));
            }

            //SerializeToJsonFile(strainAnalysis, $"Isso{parameters.IssoId}N{parameters.CheckPointNumber}Load{parameters.LoadSchema.NameShort}.json" );

            return new ResultMonad<StrainAnalysisResult>(ComposeMessage(parameters, strainAnalysis, data.TrianglesToCache));
        }

        public static void SerializeToJsonFile(object obj, string filename = "output.json")
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,  // Pretty-print with indentation
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase  // Optional: camelCase keys
            };

            string json = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(filename, json);
            Console.WriteLine($"JSON serialized to {Path.GetFullPath(filename)}");
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
                    StrainCalculationGroupType = Enums.StrainCalculationGroupTypeEnum.Unknown
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
