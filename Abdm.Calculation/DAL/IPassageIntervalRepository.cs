using System.Threading.Tasks;
using Abdm.Calculation.Models;

namespace Abdm.Calculation.DAL
{
    public interface IPassageIntervalRepository
    {
        Task<PassageInterval[]> GetPassageIntervals(long issoId);
    }
}
