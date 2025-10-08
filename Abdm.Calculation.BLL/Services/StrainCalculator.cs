using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.Algorithmic;
using Abdm.Calculation.BLL.Models.Parameters;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Сервис для рассчетов напряжения в колонне
    /// </summary>
    /// <param name="profileYZService"></param>
    public class StrainCalculator(IProfileYZService profileYZService) : IStrainCalculator
    {
        public IEnumerable<StrainResult> GetStrainResult(PassTypeDataModel calculationData, RoadRule[] roadRules)
        {
            throw new NotImplementedException();
        }




        public StrainResult CalculateColumnModel(
            [DisallowNull] VehicleTrajectory[] vehicleTrajectories,
            LoadSchema loadSchema, 
            RoadRules roadRules)
        {
            var column = new StrainDataContainer(vehicleTrajectories);

            if (vehicleTrajectories.Length == 0)
            {
                return column;
            }

            //foreach (var trajectory in vehicleTrajectories) 
            //{
            //    CalculateStrain(column, trajectory);
            //}

            return column;
        }


       

        private void CalculateStrain(StrainResult column, VehicleTrajectory trajectory, LoadSchema loadSchema)
        {
            var maxStrainPosition = profileYZService.GetMaxZPosition(trajectory.Center);



            //var strain = CalculateStrainInPositions(trajectory, maxStainPosition);
            //column.Strain.Add(strain);
            //column.StrainOneAuto.Add(strain);
        }

        //private double CalculateStrainInPositions(
        //    VehicleTrajectory trajectory, 
        //    double maxStainPosition)
        //{
        //    foreach (var strainPosition in maxStrainPositions)
        //    {
        //        trajectory.Left.
        //    }
        //}

        //private 
    }
}
