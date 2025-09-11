using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Services
{
    public interface ISurfaceDataService
    {
        Task<SurfaceData> GetSurfaceData(long IssoId, int checkpointNumber);
    }
}