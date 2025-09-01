using System;
using System.Linq;
using System.Threading.Tasks;
using Abdm.Calculation.G4;
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
                var colonna = new Colonna(interval);


            }



            return await Task.FromResult<PTCResultMessage>(new PTCResultMessage());
        }


    }
}
