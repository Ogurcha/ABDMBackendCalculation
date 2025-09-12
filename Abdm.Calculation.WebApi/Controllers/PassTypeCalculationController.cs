using System;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.WebApi.Mappers;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassTypeCalculationController(IPassTypeCalculator ptcProcessor,
        ILogger<PTCMessageHandler> logger,
        IPassTypeModelsMapper mapper) : Controller
    {
        private const string infoMsg = "PassType calculation for (IssoId = {1}, Check point number = {2}) started";
        private const string errorMsg = "Failed PassType calculation for (IssoId = {1}, Check point number = {2})";
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
                if (responseContent != null && responseContent.IssoId > 0 && responseContent.CPNumber > 0)
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
