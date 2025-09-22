using System.Threading.Tasks;
using Abdm.Calculation.WebApi.Mappers;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace Abdm.Calculation.WebApi.Controllers
{
    [ApiController]
    [Route("PassType")]
    public class PassTypeController(IPassTypeService passTypeService,
        IPassTypeModelsMapper mapper) : Controller
    {
        [HttpGet("GetPassType")]
        public async Task<ActionResult<PTCResultMessageResponseModel>> GetPassType(PTCRequestMessageRequestModel requestModel)
        {
            var data = mapper.FromDTO(requestModel);
            var responseContent = await passTypeService.GetPassType(data, new System.Threading.CancellationToken());
            return Ok(mapper.ToDTO(responseContent));
        }
    }
}
