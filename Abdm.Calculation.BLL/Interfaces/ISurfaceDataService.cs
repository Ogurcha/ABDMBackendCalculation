using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ISurfaceDataService
    {
        Task<SurfaceData> GetSurfaceData(long issoId, int checkpointNumber, CancellationToken cancellationToken);
    }
}