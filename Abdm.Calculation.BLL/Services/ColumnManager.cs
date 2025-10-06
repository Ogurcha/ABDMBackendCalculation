using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Сервис для рассчетов напряжения в колонне
    /// </summary>
    /// <param name="profileYZService"></param>
    public class ColumnManager(IProfileYZService profileYZService) : IColumnManager
    {

        public ColumnModel CalculateColumnModel(
            [DisallowNull] VehicleTrajectory[] vehicleTrajectories,
            LoadSchema loadSchema, 
            RoadRules roadRules)
        {
            var column = new ColumnModel(vehicleTrajectories);

            if (vehicleTrajectories.Length == 0)
            {
                return column;
            }

            foreach (var trajectory in vehicleTrajectories) 
            {
                CalculateStrain(column, trajectory);
            }

            return column;
        }        

        private void CalculateStrain(ColumnModel column, VehicleTrajectory trajectory, LoadSchema loadSchema)
        {
            var maxStrainPosition = profileYZService.GetMaxZPosition(trajectory.Center);



            var strain = CalculateStrainInPositions(trajectory, maxStainPosition);
            column.Strain.Add(strain);
            column.StrainOneAuto.Add(strain);
        }

        private double CalculateStrainInPositions(
            VehicleTrajectory trajectory, 
            double maxStainPosition)
        {
            foreach (var strainPosition in maxStrainPositions)
            {
                trajectory.Left.
            }
        }

        private 
    }
}
