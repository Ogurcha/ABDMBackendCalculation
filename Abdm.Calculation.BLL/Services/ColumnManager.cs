using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class ColumnManager() : IColumnManager
    {

        public ColumnModel CalculateColumnModel(
            [DisallowNull] VehicleTrajectory[] vehicleTrajectories, 
            LoadSchema loadSchema, 
            RoadRules roadRules)
        {
            foreach (var trajectory in vehicleTrajectories) 
            {
                double[] Ys = 
            
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

        public double[] 

    }
}
