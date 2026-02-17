using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IWorkerWrapper
    {
        Task<PassTypeCalculationResult> GetPassType(PassTypeCalculationParameters requestModel, CancellationToken cancellationToken);
    }
}
