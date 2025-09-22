using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.WebApi
{
    public interface IPassTypeService
    {
        Task<PassTypeCalculationResult> GetPassType(PassTypeCalculationParameters requestModel);
    }
}