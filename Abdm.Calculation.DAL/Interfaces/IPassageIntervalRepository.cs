using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.DAL.Interfaces
{
    public interface IPassageIntervalRepository
    {
        Task<PassageIntervalDto[]> GetPassageIntervals(long issoId, CancellationToken cancellationToken);
    }
}
