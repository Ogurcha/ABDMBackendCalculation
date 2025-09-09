using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Interfaces;

public interface IPassageIntervalService
{
    public Task<PassageInterval[]> GetPassageIntervals(long issoId);

    public double[] CalculateDistinctXPositionsIncludingWheelOffsets(
        double[] distinctXs, 
        PassageInterval passageInterval, 
        AxleModel[] axles, 
        double carWidth
        );
}
