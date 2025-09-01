using System;
using System.Linq;
using System.Threading.Tasks;
using Abdm.Calculation.G4;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.IntervalCalculation;
using Abdm.Calculation.Models;
using Abdm.Calculation.PassTypeCalculation.DTO;
using Abdm.Calculation.RoadRules;

namespace Abdm.Calculation.ColumnCalculation
{
    public class PassTypeCalculator (
        IPassageIntervalManager passageIntervalManager,
        IMeshProcessor meshProcessor,
        IRoadRulesManager roadRulesManager
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

                    var smoothPoints = SmoothPointsFactory.BuildByZ(profileYZ.ToArray());

                    column.Points[i] = smoothPoints;
                }
            }
            return await Task.FromResult<PTCResultMessage>(new PTCResultMessage());
        }


    }
}
