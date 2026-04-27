using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Default;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis.Strategies
{
    public class DefaultStrainAnalyser : ISAStrategy
    {
        public StrainCalculationGroupTypeEnum[] StrainCalculationGroupTypes { get => [
            StrainCalculationGroupTypeEnum.Default,
            StrainCalculationGroupTypeEnum.Pillar,
            StrainCalculationGroupTypeEnum.SteelConcrete,
        ];}

        public AnalysisSummary Analyse(AnalysisSummary analysis, VehicleRollingResult vehicleRollingResult)
        {
            var defaults = new List<AnalysisDefault>();
            var dataModel = vehicleRollingResult.DataModel;

            foreach (var strain in vehicleRollingResult.StrainResults)
            {
                defaults.Add(new AnalysisDefault { 
                    HasSafetyLine = strain.RoadRuleRef.HasSafetyLine, 
                    Vehicles = GetAnalysisVehicles(strain.Strain, dataModel).ToArray() ,
                    IsForward = strain.Strain.Any(x => x.IsDirectionForward)
                });
                if (strain.Strain.Any(x => x.InvertedDirectionStrain != null))
                {
                    defaults.Add(new AnalysisDefault
                    {
                        HasSafetyLine = strain.RoadRuleRef.HasSafetyLine,
                        Vehicles = GetAnalysisVehicles(strain.Strain.Select(x => x.InvertedDirectionStrain).Where(x => x != null).Cast<VehicleStrain>(), dataModel).ToArray(),
                        IsForward = strain.Strain.Select(x => x.InvertedDirectionStrain).Any(x => x?.IsDirectionForward == true),
                    });
                }
            }

            analysis.Lambda = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.Lambda);
            analysis.MyStrength = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.MyStrength);
            analysis.ConstLoad = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.ConstLoad);
            analysis.PedestrianLoad = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.PedestrianLoad);
            analysis.OtherLoad = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.OtherLoad);
            analysis.Default = defaults.OrderByDescending(x => x.HasSafetyLine).OrderByDescending(x => x.IsForward).ToList();

            return analysis;
        }

        private List<AnalysisVehicle> GetAnalysisVehicles(IEnumerable<VehicleStrain> strainResults, VehicleRollingBigModel data)
        {
            var vehicles = new List<AnalysisVehicle>();
            var columnCounter = 1;
            foreach (var strain in strainResults.OrderBy(x => x.WheelStrains.Min(w => w.Position.X)))
            {
                vehicles.Add(GetAnalysisVehicle(strain, data.Intervals.First().AbsolutePositionLeft, columnCounter));
                columnCounter++;
            }

            return vehicles;
        }

        private AnalysisVehicle GetAnalysisVehicle(VehicleStrain vehicleStrain, 
            double leftIntervalStart, 
            int columNumber)
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
            ProfileVector[]? intervalProfileVectors = null; 
            if (vehicleStrain.TrafficJamStrain != null)
            {
                intervals = new List<TrafficJamStrainAnalysis>();
                var trajectory = vehicleStrain.VehicleTrajectoryRef;

                var curveLeft = trajectory.Left.Last().Value.GetYZ().ToArray();
                var curveRight = trajectory.Right.Last().Value.GetYZ().ToArray();
                var curveCenter = trajectory.Center.GetYZ().ToArray();

                var positivePiecesLeft = MathExtensions.GetPositvePieces(curveLeft);
                var positivePiecesRight = MathExtensions.GetPositvePieces(curveRight);
                var positivePiecesCenter = MathExtensions.GetPositvePieces(curveCenter);
                var leftPieces = positivePiecesLeft.Select<Vector2D, (double BeginY, double EndY)>(x => new(x.X, x.Y)).ToArray();
                var rightPieces = positivePiecesRight.Select<Vector2D, (double BeginY, double EndY)>(x => new(x.X, x.Y)).ToArray();
                var centerPieces = positivePiecesCenter.Select<Vector2D, (double BeginY, double EndY)>(x => new(x.X, x.Y)).ToArray();

                for (var i = 0; 
                    i < Math.Min(leftPieces.Length, Math.Min(rightPieces.Length, centerPieces.Length)); 
                    i++) {

                    var left = leftPieces[i];
                    var right = rightPieces[i];
                    var center = centerPieces[i];

                    var minusKiller = 0d;
                    if (left.BeginY < 0 || right.BeginY < 0)
                    {
                        minusKiller = Math.Min(left.BeginY, right.BeginY);
                    }

                    var leftLength = left.EndY - left.BeginY;
                    var rightLength = right.EndY - right.BeginY;
                    var centerLength = center.EndY - center.BeginY;

                    intervals.Add(new TrafficJamStrainAnalysis
                    {
                        Number = columNumber,
                        LeftIntervalStart = MathExtensions.ToDecimal(left.BeginY - minusKiller),
                        LeftIntervalEnd = MathExtensions.ToDecimal(left.EndY - minusKiller),
                        LeftIntervalLength = MathExtensions.ToDecimal(leftLength),
                        LeftIntervalStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.LeftStrain),
                        RightIntervalStart = MathExtensions.ToDecimal(right.BeginY - minusKiller),
                        RightIntervalEnd = MathExtensions.ToDecimal(right.EndY - minusKiller),
                        RightIntervalLength = MathExtensions.ToDecimal(rightLength),
                        RightIntervalStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.RightStrain),
                        SumStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.SumStrain),
                        CenterIntervalStart = MathExtensions.ToDecimal(center.BeginY - minusKiller),
                        CenterIntervalEnd = MathExtensions.ToDecimal(center.EndY - minusKiller),
                        CenterIntervalLength = MathExtensions.ToDecimal(centerLength),
                        LeftIntervalVolume = MathExtensions.ToDecimal(MathExtensions.CalculateAreaUnderCurve(curveLeft)),
                        RightIntervalVolume = MathExtensions.ToDecimal(MathExtensions.CalculateAreaUnderCurve(curveRight)),
                    });
                }

                intervalProfileVectors = curveCenter.Select<Vector2D, ProfileVector>(x => (MathExtensions.ToDecimal(x.X), MathExtensions.ToDecimal(x.Y))).ToArray();
            }

            return new AnalysisVehicle
            {
                ColumnNumber = columNumber,
                VehicleNumber = 1, //TODO: добавить поддержку нескольких машин в колонне
                Wheels = wheels,
                Intervals = intervals,
                PositionX = MathExtensions.ToDecimal(vehicleStrain.WheelStrains.Average(x => x.Position.X) - leftIntervalStart),
                PositionY = MathExtensions.ToDecimal(vehicleStrain.WheelStrains.Min(x => x.Position.Y)),
                SumStrain = wheels.Sum(w => w.Strain),
                TotalStrain = MathExtensions.ToDecimal(vehicleStrain.SumStrain * vehicleStrain.Coefficient),
                IntervalProfileVectors = intervalProfileVectors,
                LambdaSmall = MathExtensions.ToDecimal(33), //TODO: добавить поддержку реального lambdaSmall
                DynamicCoefficient = MathExtensions.ToDecimal(1.1), //TODO: добавить поддержку реального динамического коэффициента
            };
        }

        private WheelAnalysis GetAnalysisWheel(WheelStrain wheelStrain, 
            double leftIntervalStart, 
            int number, 
            int subNumber)
        {
            return new WheelAnalysis()
            {
                Number = number,
                SubNumber = subNumber,
                Strain = MathExtensions.ToDecimal(wheelStrain.Strain),
                PositionX = MathExtensions.ToDecimal(wheelStrain.Position.X - leftIntervalStart),
                PositionY = MathExtensions.ToDecimal(wheelStrain.Position.Y),
                Z = MathExtensions.ToDecimal(wheelStrain.Strain / wheelStrain.AxleRef.WheelWeight),
                Weight = MathExtensions.ToDecimal(wheelStrain.AxleRef.WheelWeight),
                Pressure = MathExtensions.ToDecimal(wheelStrain.Strain / wheelStrain.AxleRef.Wy / wheelStrain.AxleRef.Wx),

                FootPrintSizeFirst = 0.56m,
                FootPrintSizeSecond = 0.96m,
                ZVolume = MathExtensions.ToDecimal(wheelStrain.Strain / wheelStrain.AxleRef.WheelWeight / 0.56 / 0.96),
            };
        }
    }
}
