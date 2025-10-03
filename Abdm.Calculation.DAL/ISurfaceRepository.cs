namespace Abdm.Calculation.DAL
{
    public interface ISurfaceRepository
    {
        Task<byte[]?> GetSurfaceData(long issoId, int checkpointNumber, CancellationToken cancellationToken);
    }
}