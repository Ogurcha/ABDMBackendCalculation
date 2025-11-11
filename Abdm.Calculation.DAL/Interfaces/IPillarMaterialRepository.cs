using Abdm.Calculation.DAL.DataTransferObjects;

namespace Abdm.Calculation.DAL.Interfaces
{
    public interface IPillarMaterialRepository
    {
        Task<PillarMaterialDto?> GetPillarMaterial(long issoId, int checkpointNumber, CancellationToken cancellationToken);
    }
}