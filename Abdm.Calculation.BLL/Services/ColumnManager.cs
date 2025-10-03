using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class ColumnManager(IProfileYZService profileYZService) : IColumnManager
    {
        /// <summary>
        /// ограничим максимальное количество ТС на уровне менеджера, 
        /// чтобы не повесить калькуляцию надолго, если что-то пойдёт не так
        /// </summary>
        private const int VehicleInColumnLimiter = 7;

        public ColumnModel CalculateColumnModel(
            [DisallowNull] VehicleTrajectory[] vehicleTrajectories, 
            LoadSchema loadSchema, 
            RoadRules roadRules)
        {
            var column = new ColumnModel(vehicleTrajectories);
            
            foreach (var trajectory in vehicleTrajectories) 
            {
                var maxVehicles = Math.Min(roadRules.MaxAutoInColumn, VehicleInColumnLimiter);

                if (maxVehicles <= 1)
                {
                    var maxStrainPositions = new List<double> { profileYZService.GetMaxZPosition(trajectory.Center) };

                    var strain = CalculateStrainInPositions(trajectory, maxStrainPositions);
                    column.Strain.Add(strain);
                    column.StrainOneAuto.Add(strain);
                }
                else
                {
                    var heightTree = new SortedDictionary<float, float>(
                    profileYZService.GetFloatYZFromProfile(trajectory.Center)
                    .Select(v => new KeyValuePair<float, float>(v.Y, v.X))
                    .ToDictionary());

                    var maxStrainPositions = GetMaxStrainPositions(heightTree, trajectory.Center, maxVehicles, []);

                    var strain = CalculateStrainInPositions(trajectory, [.. maxStrainPositions.Select(x => (double)x)]);
                    var strainOneVehicle = CalculateStrainInPositions(trajectory, [maxStrainPositions.First()]);
                    column.Strain.Add(strain);
                    column.StrainOneAuto.Add(strainOneVehicle);
                }
            }
            return column;
        }

        private List<float> GetMaxStrainPositions(
            SortedDictionary<float, float> heightTree, 
            ProfileYZ profileYZ, 
            int vehiclesToPlace, 
            List<float> placedVehicles)
        {
            if (vehiclesToPlace == 0)
            {
                return placedVehicles;
            }
            vehiclesToPlace--;

        }

        private double CalculateStrainInPositions(
            VehicleTrajectory trajectory, 
            List<double> maxStrainPositions)
        {
            foreach (var strainPosition in maxStrainPositions)
            {
                trajectory.Left.
            }

            for (var i = 0; i < vehicleXPositions.Length; i++)
            {
                var X = vehicleXPositions[i];


                if (profileVectors == null)
                {
                    continue;
                }
                column.VehicleTrajectories.Le

                column.ProfileVectors[i] = profileVectors;

                var strainList = mesh.Data.DistinctYs
                    .Select(Y => strainManager.GetStrain(data, profileVectors, Y))
                    .Order().ToList();

                //TODO: ABDMP-369 - Учитывать расстояние между авто. Пока будем считать, что они могут стоять друг на друге. Пока забьем на расстояние между ними, и то, что они все не поместятся на иссо, так как это в любом случае не приведёт к ложно положительному прогнозу
                for (int j = 0; j < roadRules.MaxAutoInColumn; j++)
                {
                    var highestStrain = strainList.Last();
                    if (j == 0)
                    {
                        column.StrainOneAuto[i] += highestStrain;
                    }

                    column.Strain[i] += highestStrain;
                    strainList.Remove(highestStrain);
                }
            }

            return column;
        }

    }
}
