using System.Threading.Tasks;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Abdm.Calculation.WebApi.Controllers
{
    [ApiController]
    [Route("/api/vehicleRolling")]
    public class VehicleRollingController(IPassTypeService passTypeService) : Controller
    {
        [HttpGet("passType")]
        public async Task<ActionResult<PassTypeCalculationResponse>> GetPassType(PassTypeCalculationRequest requestModel)
        {
            var data = requestModel.Adapt<PassTypeCalculationParameters>();
            var responseContent = await passTypeService.GetPassType(data, new System.Threading.CancellationToken());
            return Ok(responseContent.Adapt<PassTypeCalculationResponse>());
        }

        [HttpGet("maximumStrain")]
        public async Task<ActionResult<MaximumStrainCalculationResponse>> GetMaximumStrain(PassTypeCalculationRequest requestModel)
        {
            var data = requestModel.Adapt<PassTypeCalculationParameters>();
            var responseContent = await passTypeService.GetPassType(data, new System.Threading.CancellationToken());
            return Ok(responseContent.Adapt<PassTypeCalculationResponse>());
        }
    }
}
