using System;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.PassTypeCalculation;
using Abdm.Calculation.WebApi.Handlers;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.WebApi
{
    public class PassTypeService(
        IPassTypeCalculationCoordinator ptcCoordinator,
        ILogger<PTCMessageHandler> logger
        ) : IPassTypeService
    {
        private const string infoMsg = "PassType calculation for (IssoId = {0}, Check point number = {1}) started";
        private const string exceptionMsg = "Failed PassType calculation for (IssoId = {0}, Check point number = {1})";
        private const string producerErrorMsg = "Message producer failed to send message";
        private const string errorMsg = "Error while calculating PassType";

        public async Task<PTCResultMessage> GetPassType(PTCRequestMessage requestModel)
        {
            try
            {
                logger.LogInformation(string.Format(infoMsg, requestModel.IssoId, requestModel.CPNumber));
                var result = await ptcCoordinator.GetPassType(requestModel);
                if (result.IsSuccess && result.Data != null)
                {
                    return result.Data;    
                }
                else
                {
                    if (result.Exception != null)
                    {
                        logger.LogError(result.Exception, errorMsg);
                    }
                    return ptcCoordinator.GetFailedResponse(requestModel);
                }
            }
            catch (Exception e)
            {
                logger.LogError(string.Format(exceptionMsg, requestModel?.IssoId, requestModel?.CPNumber));
                logger.LogError(e.StackTrace);
                return ptcCoordinator.GetFailedResponse(requestModel);
            }
        }
    }
}
