using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Mapster;

namespace Abdm.Calculation.BLL.Coordinators
{
    public class StrainAnalysisCalulationCoordinator(
        IBaseVehicleRollingCalculationCoordinator baseCoordinator,
        IStrainAnalyser strainAnalyser
        ) : ICoordinator<PassTypeCalculationParameters, StrainAnalysisResult>
    {
        public async Task<ResultExceptionContainer<StrainAnalysisResult>> Run(
            [DisallowNull] PassTypeCalculationParameters parameters,
            CancellationToken cancellationToken)
        {
            var bigDataResult = await baseCoordinator.PrepareDataModel(parameters, null, cancellationToken);
            if (!bigDataResult.IsSuccess)
            {
                return new ResultExceptionContainer<StrainAnalysisResult>(bigDataResult.Exception!);
            }
            var data = bigDataResult.Result!;

            var defaultRollResult = baseCoordinator.RollAndGetStrainResult(data, cancellationToken);
            if (!defaultRollResult.IsSuccess)
            {
                return new ResultExceptionContainer<StrainAnalysisResult>(defaultRollResult.Exception!);
            }
            var defaultRoll = defaultRollResult.Result!;
            data.FlipMeshes();
            var mirroredRollResult = baseCoordinator.RollAndGetStrainResult(data, cancellationToken);
            if (!mirroredRollResult.IsSuccess)
            {
                return new ResultExceptionContainer<StrainAnalysisResult>(mirroredRollResult.Exception!);
            }
            var mirroredRoll = mirroredRollResult.Result!;

            var strainAnalysis = strainAnalyser.GetAnalysis(defaultRoll, mirroredRoll);

            //SerializeToJsonFile(strainAnalysis, $"Isso{parameters.IssoId}N{parameters.CheckPointNumber}Load{parameters.LoadSchema.NameShort}.json" );

            return new ResultExceptionContainer<StrainAnalysisResult>(strainAnalysis.Adapt<StrainAnalysisResult>());
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

        public string InfoMsg(PassTypeCalculationParameters param)
        {
            return string.Format("PassType calculation for (IssoId = {0}, Check point number = {1}) started", param.IssoId, param.CheckPointNumber);
        }

        public string ErrorMsg(PassTypeCalculationParameters param)
        {
            return string.Format("Error while calculating PassType for {0}, n = {1}", param.IssoId, param.CheckPointNumber);
        }

        public string ExceptionMsg(PassTypeCalculationParameters param)
        {
            return string.Format("Failed PassType calculation for (IssoId = {0}, Check point number = {1})", param.IssoId, param.CheckPointNumber);
        }

        public StrainAnalysisResult GetFailedResult(PassTypeCalculationParameters param)
        {
            throw new NotImplementedException();
        }
    }
}
