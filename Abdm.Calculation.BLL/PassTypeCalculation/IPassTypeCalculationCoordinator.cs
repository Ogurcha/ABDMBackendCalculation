using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.PassTypeCalculation
{
    public interface IPassTypeCalculationCoordinator
    {
        Task<ResultExceptionContainer<PTCResultMessage>> GetPassType(PTCRequestMessage data, CancellationToken cancellationToken);

        PTCResultMessage GetFailedResponse(PTCRequestMessage? data);
    }
}
