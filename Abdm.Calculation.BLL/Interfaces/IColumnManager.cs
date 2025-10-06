using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IColumnManager
    {
        ColumnModel CalculateColumnModel(
            [DisallowNull] VehicleTrajectory[] vehicleTrajectories,
            LoadSchema loadSchema,
            RoadRules roadRules);
    }
}