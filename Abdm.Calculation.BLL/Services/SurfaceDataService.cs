using Abdm.Calculation.DAL;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Services
{
    public class SurfaceDataService(ISurfaceRepository repository) : ISurfaceDataService
    {
        public async Task<SurfaceData> GetSurfaceData(long IssoId, int checkpointNumber)
        {
            return await repository.GetSurfaceData(IssoId, checkpointNumber);
        }
    }
}
