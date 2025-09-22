using System.Threading;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.WebApi
{
    public interface IPassTypeService
    {
        Task<PTCResultMessage> GetPassType(PTCRequestMessage requestModel, CancellationToken cancellationToken);
    }
}
