using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL
{
    public interface IPassTypeService
    {
        Task<PTCResultMessage> GetPassType(PTCRequestMessage requestModel, CancellationToken cancellationToken);
    }
}
