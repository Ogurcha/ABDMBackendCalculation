using Abdm.Calculation.BLL.Entities;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces;

public interface IPassageIntervalService
{
    public Task<PassageIntervalModel[]> GetPassageIntervals(long issoId, CancellationToken cancellationToken);

    public VehicleXPosition[] CalculateVehiclePositionsIncludingWheelOffsets(
        double[] distinctXs,
        PassageIntervalModel passageInterval,
        LoadSchema loadSchema,
        RoadRules roadRules
        );
}
