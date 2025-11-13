using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPassTypeCalculationCoordinator
    {
        Task<ResultExceptionContainer<PassTypeCalculationResult>> GetPassType(PassTypeCalculationParameters data, CancellationToken cancellationToken);

        PassTypeCalculationResult GetErrorResponse(PassTypeCalculationParameters? data);
    }
}
