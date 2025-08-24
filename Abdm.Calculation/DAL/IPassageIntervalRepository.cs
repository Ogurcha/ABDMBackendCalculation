using System.Threading.Tasks;

namespace Abdm.Calculation.DAL
{
    public interface IPassageIntervalRepository
    {
        Task<double[]> GetPassageIntervals(long issoId);
    }
}