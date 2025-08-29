using System.Threading.Tasks;
using Abdm.Calculation.Models;

namespace Abdm.Calculation.BusinessLogic
{
    public interface IPassageIntervalManager
    {
        Task<double[]> GetDistinctXsWithWheels(double[] distinctXs, PassageInterval[] passageIntervals, Axle[] axles, double nagruzkaPassageWidth);
        Task<PassageInterval[]> GetPassageIntervals(long issoId);
    }
}