using Abdm.Calculation.DAL.DataTransferObjects;

namespace Abdm.Calculation.DAL
{
    public interface ISurfaceRepository
    {
        Task<SurfaceRawDataDto?> GetSurfaceData(long issoId, int checkpointNumber, CancellationToken cancellationToken);
    }
}