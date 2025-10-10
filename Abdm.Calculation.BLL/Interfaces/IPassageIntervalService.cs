using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces;

public interface IPassageIntervalService
{
    public Task<PassageInterval[]> GetPassageIntervals(long issoId,
        double globalPositionShift,
        CancellationToken cancellationToken);
}
