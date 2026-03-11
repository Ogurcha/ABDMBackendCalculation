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
    public class VehicleRollingController(
        ICanWork<PassTypeCalculationParameters, PassTypeCalculationResult> passTypeCalculator,
        ICanWork<StrainAnalysisParameters, StrainAnalysisResult> strainAnalyser) : Controller
    {
        [HttpGet("pass-type")]
        public async Task<ActionResult<PassTypeCalculationResponse>> GetPassType(PassTypeCalculationRequest requestModel)
        {
            var data = requestModel.Adapt<PassTypeCalculationParameters>();
            var responseContent = await passTypeCalculator.Run(data, new System.Threading.CancellationToken());
            return Ok(responseContent.Adapt<PassTypeCalculationResponse>());
        }

        [HttpGet("strain-analysis")]
        public async Task<ActionResult<AnalyseStrainCalculationResponse>> GetAnalyseStrain(StrainAnalysisCalculationRequest requestModel)
        {
            var data = requestModel.Adapt<StrainAnalysisParameters>();
            var responseContent = await strainAnalyser.Run(data, new System.Threading.CancellationToken());
            return Ok(responseContent.Adapt<AnalyseStrainCalculationResponse>());
        }
    }
}
