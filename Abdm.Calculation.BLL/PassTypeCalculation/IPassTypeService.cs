using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL
{
    public interface IPassTypeService
    {
        Task<PassTypeCalculationResult> GetPassType(PassTypeCalculationParameters requestModel, CancellationToken cancellationToken);
    }
}
