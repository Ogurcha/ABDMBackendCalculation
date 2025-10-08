using Abdm.Calculation.BLL.Models.Algorithmic;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Interfaces;

public interface IPassageIntervalService
{
    public Task<PassageInterval[]> GetPassageIntervals(long issoId,
        double globalPositionShift,
        CancellationToken cancellationToken);

    public VehicleXPosition[] CalculateVehiclePositionsIncludingWheelOffsets(
        double[] distinctXs,
        PassageInterval passageInterval,
        LoadSchema loadSchema,
        RoadRule[] roadRules
        );
}
