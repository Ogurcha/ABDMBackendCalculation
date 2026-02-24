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

            var result = new AnalysisSummary { Vehicles = maxStrainResult.Strain.Select(GetAnalysisVehicle)
                .OrderBy(x => x.Axles.Average(x => x.PositionX)).ToList()};

            return result;
        }

        private AnalysisVehicle GetAnalysisVehicle(VehicleStrain vehicleStrain)
        {
            var wheelAnalysis = vehicleStrain.WheelStrains.Select(GetAnalysisWheel).ToList();
            return new AnalysisVehicle
            {
                Axles = wheelAnalysis
            };
        }

        private AnalysisWheel GetAnalysisWheel(WheelStrain wheelStrain)
        {
            return new AnalysisWheel()
            {
                Height = decimal.Round((decimal)wheelStrain.AxleRef.Wy, 2),
                Width = decimal.Round((decimal)wheelStrain.AxleRef.Wx, 2),
                Strain = decimal.Round((decimal)wheelStrain.Strain, 2),
                PositionX = decimal.Round((decimal)wheelStrain.Position.X, 2),
                PositionY = decimal.Round((decimal)wheelStrain.Position.Y, 2),
                Z = decimal.Round((decimal)(wheelStrain.Strain / wheelStrain.AxleRef.WheelWeight), 2),
                Weight = decimal.Round((decimal)wheelStrain.AxleRef.WheelWeight, 2),
                Pressure = decimal.Round((decimal)(wheelStrain.Strain / wheelStrain.AxleRef.Wy / wheelStrain.AxleRef.Wx), 2)
            };
        }
    }
}
