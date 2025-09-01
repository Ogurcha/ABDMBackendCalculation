using System.Threading.Tasks;
using Abdm.Calculation.Models;

namespace Abdm.Calculation.IntervalCalculation
{
    public interface IPassageIntervalManager
    {
        Task<PassageInterval[]> GetPassageIntervals(long issoId);

        double[] GetDistinctXsWithWheels(
            double[] distinctXs, 
            PassageInterval passageInterval, 
            Axle[] axles, 
            double carWidth
            );
    }
}
