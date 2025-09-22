using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;

namespace Abdm.Calculation.WebApi.Mappers
{
    public interface IPassTypeModelsMapper
    {
        PassTypeCalculationParameters FromDTO(PassTypeCalculationRequest dto);

        PassTypeCalculationResponse ToDTO(PassTypeCalculationResult model);
    }
}
