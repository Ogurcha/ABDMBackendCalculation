using System;
using System.Linq;
using System.Threading.Tasks;
using Abdm.Calculation.DAL;
using Abdm.Calculation.G4;
using Abdm.Calculation.Models;
using Abdm.Calculation.PassTypeCalculation.DTO;

namespace Abdm.Calculation.ColumnCalculation
{
    public class PassTypeCalculator (
        IPassageIntervalRepository passageIntervalRepository,
        IMeshProcessor meshProcessor
        ) : IPassTypeCalculator
    {
        
        public async Task<PTCResultMessage> CalculatePassType(PTCRequestMessage data)
        {
            var intervals = await passageIntervalRepository.GetPassageIntervals(data.IssoId);
            if (intervals?.Any() != true)
                throw new Exception("Passage intervals for this isso have not been found");

            var mesh = meshProcessor.GetMeshFromPoints(data.Surface.SurfacePoints);

            foreach (var interval in intervals)
            {
                var colonna = new Colonna(interval);


            }



            return await Task.FromResult<PTCResultMessage>(new PTCResultMessage());
        }


    }
}
