using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.PassTypeCalculation
{
    public interface IPassTypeCalculationCoordinator
    {
        Task<ResultExceptionContainer<PassTypeCalculationResult>> GetPassType(PassTypeCalculationParameters data, CancellationToken cancellationToken);

        PassTypeCalculationResult GetFailedResponse(PassTypeCalculationParameters? data);
    }
}
