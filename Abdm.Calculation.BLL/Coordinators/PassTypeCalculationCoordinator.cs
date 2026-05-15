using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Coordinators
{
    public class PassTypeCalculationCoordinator (
        IBaseVehicleRollingCalculationCoordinator baseCoordinator,
        IPassTypeResolverFactory resolverFactory
        ) : ICoordinator<PassTypeCalculationParameters, PassTypeCalculationResult>
    {
        private const string passTypeResolverNotFoundErrorMessage = "Pass type resolver for given load were not found";
        private const string strainIsNaNErrorMessage = "Calculation error. Strain equals Double.NaN";
        private const string cantGetValidStrainResults = "Calculation error. Can't get valid Strain results";

        public async Task<ResultExceptionContainer<PassTypeCalculationResult>> Run(
            [DisallowNull] PassTypeCalculationParameters parameters, 
            CancellationToken cancellationToken)
        {

            var isMirroredByZ = parameters.Surface.MyStrength < 0;
            var bigData = await baseCoordinator.PrepareDataModel(parameters, isMirroredByZ, cancellationToken);
            if (!bigData.IsSuccess)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(bigData.Exception!);
            }

            var strainsContainer = baseCoordinator.RollAndGetStrainResult(bigData.Result!, cancellationToken);
            if (!strainsContainer.IsSuccess)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(strainsContainer.Exception!);
            }
            var strainResults = strainsContainer.Result!.StrainResults;
            var dataModel = strainsContainer.Result.DataModel;

            if (!strainResults.Any())
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(cantGetValidStrainResults));
            }
            if (strainResults.Any(x => x.TotalStrain == double.NaN))
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(strainIsNaNErrorMessage));
            }
            var ptr = resolverFactory.GetPassTypeResolver(dataModel.Data.Surface.StrainCalculationGroupType);
            if (ptr == null)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(passTypeResolverNotFoundErrorMessage));
            }
            var resultPassType = ptr.Resolve(strainResults, dataModel.Data);

            var response = ComposeMessage(resultPassType, parameters);

            return new ResultExceptionContainer<PassTypeCalculationResult>(response);
        }

        public PassTypeCalculationResult GetFailedResult(PassTypeCalculationParameters? data)
        {
            if (data == null)
            {
                return new PassTypeCalculationResult
                {
                    IssoId = default,
                    CPNumber = default,
                    Allowed = AllowedEnum.Undefined,
                    Intervals = [],
                    LoadId = default,
                    Direction = default,
                    Snip = default,
                    PassType = PassTypeEnum.Unknown
                };
            }
            else
            {
                return new PassTypeCalculationResult
                {
                    IssoId = data.IssoId,
                    CPNumber = data.CheckPointNumber,
                    Allowed = AllowedEnum.Undefined,
                    Intervals = [],
                    LoadId = data.LoadId,
                    Direction = data.Direction,
                    Snip = data.Snip,
                    PassType = PassTypeEnum.Unknown
                };
            } 
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

        private PassTypeCalculationResult ComposeMessage(PassTypeEnum resultPassType, PassTypeCalculationParameters data)
        {
            AllowedEnum allowed = resultPassType switch
            {
                PassTypeEnum.NoLimit => AllowedEnum.Allowed,
                PassTypeEnum.WithoutPedestrian
                or PassTypeEnum.MaxSpeed10
                or PassTypeEnum.SingleAutoOnly
                or PassTypeEnum.SingleOnlyAndPlace => AllowedEnum.Restricted,
                PassTypeEnum.Denied => AllowedEnum.Denied,
                PassTypeEnum.Unknown or _ => AllowedEnum.Denied,
            };

            return new PassTypeCalculationResult
            {
                Allowed = allowed,
                CPNumber = data.CheckPointNumber,
                Direction = data.Direction,
                Intervals = [],
                IssoId = data.IssoId,
                PassType = resultPassType,
                LoadId = data.LoadId
            };
        }
    }
}
