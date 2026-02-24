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
                Height = wheelStrain.AxleRef.Wy,
                Width = wheelStrain.AxleRef.Wx,
                Strain = wheelStrain.Strain,
                PositionX = wheelStrain.Position.X,
                PositionY = wheelStrain.Position.Y,
                Z = wheelStrain.Strain / wheelStrain.AxleRef.WheelWeight,
                Weight = wheelStrain.AxleRef.WheelWeight,
                Pressure = wheelStrain.Strain / wheelStrain.AxleRef.Wy / wheelStrain.AxleRef.Wx
            };
        }
    }
}
