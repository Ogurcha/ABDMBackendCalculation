
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.DAL
{
    public interface ISurfaceRepository
    {
        Task<SurfaceData> GetSurfaceData(long issoId, int checkpointId);
    }
}