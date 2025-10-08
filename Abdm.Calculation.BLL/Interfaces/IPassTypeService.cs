using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IPassTypeService
    {
        Task<PassTypeCalculationResult> GetPassType(PassTypeCalculationParameters requestModel, CancellationToken cancellationToken);
    }
}
