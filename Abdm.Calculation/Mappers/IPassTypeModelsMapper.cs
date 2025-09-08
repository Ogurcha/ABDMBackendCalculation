using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.WebApi.RequestModels;
using Abdm.Calculation.WebApi.ResponseModels;

namespace Abdm.Calculation.WebApi.Mappers
{
    public interface IPassTypeModelsMapper
    {
        PTCRequestMessage FromDTO(PTCRequestMessageRequestModel dto);

        PTCResultMessageResponseModel ToDTO(PTCResultMessage model);
    }
}
