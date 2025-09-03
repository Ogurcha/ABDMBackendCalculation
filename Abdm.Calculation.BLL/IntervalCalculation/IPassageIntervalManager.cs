using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.IntervalCalculation;

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
