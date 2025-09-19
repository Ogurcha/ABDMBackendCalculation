using System;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.PassTypeCalculation;
using Abdm.Calculation.WebApi.Mappers;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.WebApi.Controllers
{
    [ApiController]
    [Route("PassType")]
    public class PassTypeController(IPassTypeCalculator ptcProcessor,
        ILogger<PTCMessageHandler> logger,
        IPassTypeModelsMapper mapper) : Controller
    {
        private const string infoMsg = "PassType calculation for (IssoId = {0}, Check point number = {1}) started";
        private const string errorMsg = "Failed PassType calculation for (IssoId = {0}, Check point number = {1})";
        private const string producerErrorMsg = "Message producer failed to send message";

        [HttpGet]
        public async Task<ActionResult<PTCResultMessageResponseModel>> Calculate(PTCRequestMessageRequestModel requestModel)
        {
            PTCResultMessage responseContent = null;
            PTCRequestMessage message = null;
            try
            {
                message = mapper.FromDTO(requestModel);
                logger.LogInformation(string.Format(infoMsg, message.IssoId, message.CPNumber));
                responseContent = await ptcProcessor.CalculatePassType(message);
                return Ok(mapper.ToDTO(responseContent));
            }
            catch (Exception e)
            {
                logger.LogError(string.Format(errorMsg, message?.IssoId, message?.CPNumber));
                if (responseContent != null && responseContent.IsValidResponse)
                {
                    try
                    {
                        return BadRequest(mapper.ToDTO(responseContent));
                    }
                    catch
                    {
                        logger.LogError(e, producerErrorMsg);
                    }
                }
            }
            return NoContent();
        }
    }
}
