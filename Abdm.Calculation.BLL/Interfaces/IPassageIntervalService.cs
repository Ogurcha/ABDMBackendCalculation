using Abdm.Calculation.BLL.Entities;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces;

public interface IPassageIntervalService
{
    public Task<PassageIntervalModel[]> GetPassageIntervals(long issoId);

    public double[] CalculateDistinctXPositionsIncludingWheelOffsets(
        double[] distinctXs,
        PassageIntervalModel passageInterval, 
        AxleModel[] axles, 
        double carWidth
        );
}
