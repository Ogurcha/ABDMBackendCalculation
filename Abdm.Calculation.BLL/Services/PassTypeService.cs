using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.BLL.Services
{
    public class PassTypeService(
        IPassTypeCalculationCoordinator ptcCoordinator,
        ILogger<PassTypeService> logger
        ) : IPassTypeService
    {
        private const string infoMsg = "PassType calculation for (IssoId = {0}, Check point number = {1}) started";
        private const string exceptionMsg = "Failed PassType calculation for (IssoId = {0}, Check point number = {1})";
        private const string errorMsg = "Error while calculating PassType for {0}, n = {1}";

        public async Task<PassTypeCalculationResult> GetPassType(PassTypeCalculationParameters requestModel, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation(string.Format(infoMsg, requestModel.IssoId, requestModel.CheckPointNumber));
                var workerThreadTask = () => ptcCoordinator.GetPassType(requestModel, cancellationToken);
                var result = await Task.Run(workerThreadTask, cancellationToken);
                if (result.IsSuccess && result.Data != null)
                {
                    return result.Data;    
                }
                else
                {
                    if (result.Exception != null)
                    {
                        logger.LogError(
                            result.Exception,
                            string.Format(errorMsg, requestModel.IssoId, requestModel.CheckPointNumber));
                    }
                    return ptcCoordinator.GetFailedResponse(requestModel);
                }
            }
            catch (Exception e)
            {
                logger.LogError(string.Format(exceptionMsg, requestModel?.IssoId, requestModel?.CheckPointNumber));
                logger.LogError(e.StackTrace);
                return ptcCoordinator.GetFailedResponse(requestModel);
            }
        }
    }
}
