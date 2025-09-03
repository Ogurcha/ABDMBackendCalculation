using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.DAL
{
    public interface IPassageIntervalRepository
    {
        Task<PassageInterval[]> GetPassageIntervals(long issoId);
    }
}
