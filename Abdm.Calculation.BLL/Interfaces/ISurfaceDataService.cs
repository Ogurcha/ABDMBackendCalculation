using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISurfaceDataService
    {
        Task<ResultExceptionContainer<SurfaceDataDto>> GetSurfaceData(
            long issoId,
            int checkpointNumber,
            PassageInterval[] intervals,
            CancellationToken cancellationToken);
    }
}