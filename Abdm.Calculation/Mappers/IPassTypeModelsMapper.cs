using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.WebApi.DTO;

namespace Abdm.Calculation.WebApi.Mappers
{
    public interface IPassTypeModelsMapper
    {
        PTCRequestMessage FromDTO(PTCRequestMessageDTO dto);
        PTCResultMessageDTO ToDTO(PTCResultMessage model);
    }
}
