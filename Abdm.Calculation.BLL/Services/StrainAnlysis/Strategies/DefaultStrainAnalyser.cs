using Abdm.Calculation.BLL.Enums;
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
        private const int ProfileVectorsLimitCount = 50;

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

                var profileLeft = trajectory.Left.Last().Value;
                var profileRight = trajectory.Right.Last().Value;
                var profileCenter = trajectory.Center;
                var leftPieces = profileLeft.PositivePieces;
                var rightPieces = profileLeft.PositivePieces;
                var centerPieces = profileLeft.PositivePieces;

                for (var i = 0;
                    i < Math.Min(leftPieces.Length, Math.Min(rightPieces.Length, centerPieces.Length));
                    i++)
                {

                    var left = leftPieces[i];
                    var right = rightPieces[i];
                    var center = centerPieces[i];

                    var minusKiller = 0d;
                    if (left.Start < 0 || right.Start < 0)
                    {
                        minusKiller = Math.Min(left.Start, right.Start);
                    }

                    intervals.Add(new TrafficJamStrainAnalysis
                    {
                        Number = columNumber,
                        LeftIntervalStart = MathExtensions.ToDecimal(left.Start - minusKiller),
                        LeftIntervalEnd = MathExtensions.ToDecimal(left.End - minusKiller),
                        LeftIntervalLength = MathExtensions.ToDecimal(left.Length),
                        LeftIntervalStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.LeftStrain),
                        RightIntervalStart = MathExtensions.ToDecimal(right.Start - minusKiller),
                        RightIntervalEnd = MathExtensions.ToDecimal(right.End - minusKiller),
                        RightIntervalLength = MathExtensions.ToDecimal(right.Length),
                        RightIntervalStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.RightStrain),
                        SumStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.SumStrain),
                        CenterIntervalStart = MathExtensions.ToDecimal(center.Start - minusKiller),
                        CenterIntervalEnd = MathExtensions.ToDecimal(center.End - minusKiller),
                        CenterIntervalLength = MathExtensions.ToDecimal(center.Length),
                        LeftIntervalVolume = MathExtensions.ToDecimal(leftPieces.Sum(x => x.Length)),
                        RightIntervalVolume = MathExtensions.ToDecimal(rightPieces.Sum(x => x.Length)),
                    });
                }

                intervalProfileVectors = GetProfileVectors(trajectory)?.ToArray();
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
                LambdaSmall = MathExtensions.ToDecimal(vehicleStrain.LambdaSmall), 
                DynamicCoefficient = MathExtensions.ToDecimal(vehicleStrain.Coefficient),
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

        private IEnumerable<ProfileVector>? GetProfileVectors(VehicleTrajectory trajectory)
        {
            var vectors = trajectory.Center.Vectors.Values;
            if (vectors.Count == 0)
            {
                return null;
            }
            if (vectors.Count < ProfileVectorsLimitCount)
            {
                return trajectory.Center.Vectors.Values.Select<Vector2D, ProfileVector>(x => (MathExtensions.ToDecimal(x.X), MathExtensions.ToDecimal(x.Y)));
            }
            return VectorsTooMany(vectors);

            IEnumerable<ProfileVector> VectorsTooMany(IList<Vector2D> vectors)
            {
                var limit = vectors.Count / ProfileVectorsLimitCount;
                var counter = limit;
                foreach (var vector in vectors)
                {
                    if (counter == limit)
                    {
                        counter = 0;
                        yield return (MathExtensions.ToDecimal(vector.X), MathExtensions.ToDecimal(vector.Y));
                    }
                    else
                    {
                        counter++;
                    }
                }
            }
        }
    }
}
