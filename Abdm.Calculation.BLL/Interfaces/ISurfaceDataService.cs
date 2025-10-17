using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISurfaceDataService
    {
        Task<ResultExceptionContainer<SurfaceDataDto>> GetSurfaceData(long issoId, int checkpointNumber, CancellationToken cancellationToken);
    }
}