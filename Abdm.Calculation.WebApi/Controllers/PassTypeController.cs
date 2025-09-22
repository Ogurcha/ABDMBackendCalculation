using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.WebApi.Mappers;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Abdm.Calculation.WebApi.Controllers
{
    [ApiController]
    [Route("/api/passType")]
    public class PassTypeController(IPassTypeService passTypeService) : Controller
    {
        [HttpGet("PassType")]
        public async Task<ActionResult<PassTypeCalculationResponse>> GetPassType(PassTypeCalculationRequest requestModel)
        {
            var data = requestModel.Adapt<PassTypeCalculationParameters>();
            var responseContent = await passTypeService.GetPassType(data);
            return Ok(responseContent.Adapt<PassTypeCalculationResponse>());
        }
    }
}
