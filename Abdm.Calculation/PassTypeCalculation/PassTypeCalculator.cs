using System;
using System.Linq;
using System.Threading.Tasks;
using Abdm.Calculation.G4;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.IntervalCalculation;
using Abdm.Calculation.Models;
using Abdm.Calculation.PassTypeCalculation.DTO;
using Abdm.Calculation.RoadRules;
using Abdm.Calculation.StrainCalculation;

namespace Abdm.Calculation.ColumnCalculation
{
    public class PassTypeCalculator (
        IPassageIntervalManager passageIntervalManager,
        IMeshProcessor meshProcessor,
        IRoadRulesManager roadRulesManager,
        IStrainManager strainManager
        ) : IPassTypeCalculator
    {
        
        public async Task<PTCResultMessage> CalculatePassType(PTCRequestMessage data)
        {
            var intervals = await passageIntervalManager.GetPassageIntervals(data.IssoId);
            if (intervals?.Any() != true)
            {
                throw new Exception("Passage intervals for this isso have not been found");
            }

            var roadRules = roadRulesManager.RefreshRoadRules(data.IssoId, data.LadingSchema.Id);

            var mesh = meshProcessor.GetMeshFromPoints(data.Surface.SurfacePoints);

            foreach (var interval in intervals)
            {
                var column = new Column(interval);

                column.Xs = passageIntervalManager.GetDistinctXsWithWheels(
                mesh.Data.DistinctXs,
                interval,
                data.LadingSchema.Axles,
                data.LadingSchema.Width
                );

                for (var i = 0; i < column.Xs.Length; i++)
                {
                    var X = column.Xs[i];

                    var profileYZ = meshProcessor.MakeProfileYZ(mesh, X);

                    var smoothPoints = SmoothPointsFactory.Create(profileYZ.ToArray());
                    column.Points[i] = smoothPoints;

                    var strainList = mesh.Data.DistinctYs
                        .Select(Y => strainManager.GetStrain(data, smoothPoints, Y))
                        .OrderDescending().ToList();

                    //TODO: Учитывать расстояние между авто. Пока будем считать, что они могут стоять друг на друге. Пока забьем на расстояние между ними, и то, что они все не поместятся на иссо, так как это в любом случае не приведёт к ложно положительному прогнозу
                    for (int j = 0; j < roadRules.MaxAutoInColumn; i++)
                    {
                        if (j == 0)
                        {
                            column.StrainOneAuto[i] += strainList.First();
                        }

                        column.Strain[i] += strainList.First();
                        strainList.RemoveAt(0);
                    }
                }
            }

            

            return await Task.FromResult<PTCResultMessage>(new PTCResultMessage());
        }



    }
}
