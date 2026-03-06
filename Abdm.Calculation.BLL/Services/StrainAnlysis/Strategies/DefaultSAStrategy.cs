using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Default;
using Abdm.Calculation.Maths.Extensions;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis.Strategies
{
    public class DefaultSAStrategy : ISAStrategy
    {
        public StrainCalculationGroupTypeEnum[] StrainCalculationGroupTypes { get => [
            StrainCalculationGroupTypeEnum.Default,
        ];}

        public AnalysisSummary Analyse(AnalysisSummary analysis, VehicleRollingResult vehicleRollingResult)
        {
            var defaults = new List<AnalysisDefault>();
            var dataModel = vehicleRollingResult.DataModel;

            foreach (var strain in vehicleRollingResult.StrainResults)
            {
                defaults.Add(new AnalysisDefault { 
                    HasSafetyLine = strain.RoadRuleRef.HasSafetyLine, 
                    Vehicles = GetAnalysisVehicles(strain, dataModel).ToArray() 
                });
            }

            analysis.Default = defaults;

            return analysis;
        }

        private List<AnalysisVehicle> GetAnalysisVehicles(StrainResult strainResult, VehicleRollingBigModel data)
        {
            var vehicles = new List<AnalysisVehicle>();
            var columnCounter = 1;
            foreach (var strain in strainResult.Strain.OrderBy(x => x.WheelStrains.Min(w => w.Position.X)))
            {
                vehicles.Add(GetAnalysisVehicle(strain, data.Intervals.First().AbsolutePositionLeft, columnCounter));
                columnCounter++;
            }

            return vehicles;
        }

        private AnalysisVehicle GetAnalysisVehicle(VehicleStrain vehicleStrain, double leftIntervalStart, int columNumber)
        {
            var wheelCounter = 1;
            var wheels = new List<WheelAnalysis>();
            foreach (var wheelStrains in vehicleStrain.WheelStrains.OrderBy(x => x.Position.Y).GroupBy(x => x.Position.Y))
            {
                var wheelSubCounter = 1;
                foreach (var wheelStrain in wheelStrains)
                {
                    wheels.Add(GetAnalysisWheel(wheelStrain, leftIntervalStart, wheelCounter, wheelSubCounter));
                    wheelSubCounter++;
                }
                wheelCounter++;
            }

            List<TrafficJamStrainAnalysis>? intervals = null;
            if (vehicleStrain.TrafficJamStrain != null)
            {
                intervals = new List<TrafficJamStrainAnalysis>();

                for (var i = 0; i < Math.Min(vehicleStrain.TrafficJamStrain.LeftPieces.Length, vehicleStrain.TrafficJamStrain.RightPieces.Length); i++) {
                    var left = vehicleStrain.TrafficJamStrain.LeftPieces[i];
                    var right = vehicleStrain.TrafficJamStrain.RightPieces[i];

                    var leftLength = left.EndY - left.BeginY;
                    var rightLength = right.EndY - right.BeginY;

                    intervals.Add(new TrafficJamStrainAnalysis
                    {
                        Number = columNumber,
                        LeftIntervalStart = MathExtensions.ToDecimal(left.BeginY),
                        LeftIntervalEnd = MathExtensions.ToDecimal(left.EndY),
                        LeftIntervalLength = MathExtensions.ToDecimal(leftLength),
                        LeftIntervalStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.LeftStrain),
                        RightIntervalStart = MathExtensions.ToDecimal(right.BeginY),
                        RightIntervalEnd = MathExtensions.ToDecimal(right.EndY),
                        RightIntervalLength = MathExtensions.ToDecimal(rightLength),
                        RightIntervalStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.RightStrain),
                        SumStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.SumStrain)
                    });
                }
            }

            return new AnalysisVehicle
            {
                ColumnNumber = columNumber,
                Wheels = wheels,
                Intervals = intervals,
                PositionX = MathExtensions.ToDecimal(vehicleStrain.WheelStrains.Average(x => x.Position.X) - leftIntervalStart),
                PositionY = MathExtensions.ToDecimal(vehicleStrain.WheelStrains.Min(x => x.Position.Y)),
                SumStrain = wheels.Sum(w => w.Strain)
            };
        }

        private WheelAnalysis GetAnalysisWheel(WheelStrain wheelStrain, double leftIntervalStart, int number, int subNumber)
        {
            return new WheelAnalysis()
            {
                Number = number,
                SubNumber = subNumber,
                Height = MathExtensions.ToDecimal(wheelStrain.AxleRef.Wy),
                Width = MathExtensions.ToDecimal(wheelStrain.AxleRef.Wx),
                Strain = MathExtensions.ToDecimal(wheelStrain.Strain),
                PositionX = MathExtensions.ToDecimal(wheelStrain.Position.X - leftIntervalStart),
                PositionY = MathExtensions.ToDecimal(wheelStrain.Position.Y),
                Z = MathExtensions.ToDecimal(wheelStrain.Strain / wheelStrain.AxleRef.WheelWeight),
                Weight = MathExtensions.ToDecimal(wheelStrain.AxleRef.WheelWeight),
                Pressure = MathExtensions.ToDecimal(wheelStrain.Strain / wheelStrain.AxleRef.Wy / wheelStrain.AxleRef.Wx)
            };
        }
    }
}
