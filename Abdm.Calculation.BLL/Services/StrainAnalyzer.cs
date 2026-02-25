using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainAnalyzer : IStrainAnalyzer
    {
        public AnalysisSummary Analyze(
            VehicleRollingResult defaultRolling,
            VehicleRollingResult mirroredRolling,
            VehicleRollingBigModel dataModel)
        {
            var strains = defaultRolling.StrainResults.Union(mirroredRolling.StrainResults);
            var maxStrainResult = strains.OrderBy(x => x.Strain.TotalStrain).Last();

            var vehicles = new List<AnalysisVehicle>();
            var columnCounter = 1;
            foreach (var strainResult in maxStrainResult.Strain.OrderBy(x => x.WheelStrains.Min(w => w.Position.X)))
            {
                vehicles.Add(GetAnalysisVehicle(strainResult, dataModel.Intervals.First().AbsolutePositionLeft, columnCounter));
                columnCounter++;
            }

            var result = new AnalysisSummary { 
                Vehicles = vehicles
            };

            return result;
        }

        private AnalysisVehicle GetAnalysisVehicle(VehicleStrain vehicleStrain, double leftIntervalStart, int columNumber)
        {
            var number = 1;
            var wheels = new List<AnalysisWheel>();
            foreach (var wheelStrains in vehicleStrain.WheelStrains.GroupBy(x => x.Position.Y))
            {
                var subNumber = 1;
                foreach (var wheelStrain in wheelStrains)
                {
                    wheels.Add(GetAnalysisWheel(wheelStrain, leftIntervalStart, number, subNumber));
                    subNumber++;
                }
                number++;
            }
            return new AnalysisVehicle
            {
                ColumnNumber = columNumber,
                Wheels = wheels
            };
        }

        private AnalysisWheel GetAnalysisWheel(WheelStrain wheelStrain, double leftIntervalStart, int number, int subNumber)
        {
            return new AnalysisWheel()
            {
                Number = number,
                SubNumber = subNumber,
                Height = decimal.Round((decimal)wheelStrain.AxleRef.Wy, 2),
                Width = decimal.Round((decimal)wheelStrain.AxleRef.Wx, 2),
                Strain = decimal.Round((decimal)wheelStrain.Strain, 2),
                PositionX = decimal.Round((decimal)(wheelStrain.Position.X - leftIntervalStart), 2),
                PositionY = decimal.Round((decimal)wheelStrain.Position.Y, 2),
                Z = decimal.Round((decimal)(wheelStrain.Strain / wheelStrain.AxleRef.WheelWeight), 2),
                Weight = decimal.Round((decimal)wheelStrain.AxleRef.WheelWeight, 2),
                Pressure = decimal.Round((decimal)(wheelStrain.Strain / wheelStrain.AxleRef.Wy / wheelStrain.AxleRef.Wx), 2)
            };
        }
    }
}
