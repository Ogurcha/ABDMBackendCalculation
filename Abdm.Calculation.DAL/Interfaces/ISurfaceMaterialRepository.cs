using Abdm.Calculation.DAL.DataTransferObjects;

namespace Abdm.Calculation.DAL.Interfaces
{
    public interface ISurfaceMaterialRepository
    {
        Task<SurfaceMaterialDto?> GetSurfaceMaterial(long issoId, int checkpointNumber, CancellationToken cancellationToken);
    }
}