using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Services
{
    public interface ISurfaceDataService
    {
        Task<ResultExceptionContainer<SurfaceData>> GetSurfaceData(long issoId, int checkpointNumber, CancellationToken cancellationToken);
    }
}