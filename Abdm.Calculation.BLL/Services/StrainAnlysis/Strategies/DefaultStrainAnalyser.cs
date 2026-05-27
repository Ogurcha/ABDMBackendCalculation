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
        private const int ProfileVectorsLimitCount = 50;

        public StrainCalculationGroupTypeEnum[] StrainCalculationGroupTypes { get => [
            StrainCalculationGroupTypeEnum.Default,
            StrainCalculationGroupTypeEnum.Slab,
            StrainCalculationGroupTypeEnum.Pillar,
            StrainCalculationGroupTypeEnum.SteelConcrete,
        ];}

        public AnalysisSummary Analyse(AnalysisSummary analysis, VehicleRollingResult vehicleRollingResult)
        {
            var defaults = new List<AnalysisDefault>();
            var dataModel = vehicleRollingResult.DataModel;

            foreach (var strain in vehicleRollingResult.StrainResults)
            {
                var isDirectionForward = strain.Strain.First().VehicleStrains.First().IsDirectionForward;
                defaults.Add(new AnalysisDefault
                {
                    HasSafetyLine = strain.RoadRuleRef.HasSafetyLine,
                    Vehicles = GetAnalysisColumns(strain.Strain, dataModel, x => x).ToArray(),
                    IsForward = isDirectionForward,
                });
                if (strain.Strain.First().VehicleStrains.Any(x => x.InvertedDirectionStrain != null))
                {
                    defaults.Add(new AnalysisDefault
                    {
                        HasSafetyLine = strain.RoadRuleRef.HasSafetyLine,
                        Vehicles = GetAnalysisColumns(strain.Strain, dataModel, x => x.InvertedDirectionStrain).ToArray(),
                        IsForward = !isDirectionForward,
                    });
                }
            }
            FilterDefaultsForPillar(vehicleRollingResult, defaults);

            analysis.Lambda = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.Lambda);
            analysis.MyStrength = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.MyStrength);
            analysis.ConstLoad = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.ConstLoad);
            analysis.PedestrianLoad = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.PedestrianLoad);
            analysis.OtherLoad = MathExtensions.ToDecimal(vehicleRollingResult.DataModel.Data.Surface.OtherLoad);
            analysis.Default = defaults.OrderByDescending(x => x.HasSafetyLine).OrderByDescending(x => x.IsForward).ToList();

            return analysis;
        }

        private List<AnalysisVehicle> GetAnalysisColumns(IEnumerable<VehicleColumnStrain> strainResults, 
            VehicleRollingBigModel data,
            Func<VehicleStrain, VehicleStrain?> vehicleStrainRetrieveFunc)
        {
            var vehicles = new List<AnalysisVehicle>();
            var columnCounter = 1;
            foreach (var columnStrains in strainResults.OrderBy(x => x.VehicleTrajectoryRef.X))
            {
                vehicles.AddRange(GetAnalysisColumn(columnStrains, data, columnCounter, vehicleStrainRetrieveFunc));
                columnCounter++;
            }

            return vehicles;
        }

        private List<AnalysisVehicle> GetAnalysisColumn(VehicleColumnStrain columnStrain, 
            VehicleRollingBigModel data, 
            int oneBaseColumNumber,
            Func<VehicleStrain, VehicleStrain?> vehicleStrainRetrieveFunc)
        {
            var vehicles = new List<AnalysisVehicle>();
            for (int vehicleCounter = 0; vehicleCounter < columnStrain.VehicleStrains.Length; vehicleCounter++)
            {
                var vehicle = GetAnalysisVehicle(columnStrain,
                    data.Intervals.First().AbsolutePositionLeft,
                    oneBaseColumNumber,
                    vehicleCounter,
                    vehicleStrainRetrieveFunc);
                if (vehicle != null)
                {
                    vehicles.Add(vehicle);
                }
            }

            return vehicles;
        }

        private AnalysisVehicle? GetAnalysisVehicle(VehicleColumnStrain columnStrain,
            double leftIntervalStart,
            int oneBaseColumNumber,
            int zeroBaseVehicleNumber,
            Func<VehicleStrain, VehicleStrain?> vehicleStrainRetrieveFunc)
        {
            var wheelCounter = 1;
            var wheels = new List<WheelAnalysis>();
            var vehicleStrain = vehicleStrainRetrieveFunc(columnStrain.VehicleStrains[zeroBaseVehicleNumber]);
            if (vehicleStrain == null)
            {
                return null;
            }
            double? wheelArea = null;
            foreach (var wheelStrains in vehicleStrain.WheelStrains.OrderBy(x => x.Position.Y).GroupBy(x => x.Position.Y))
            {
                var wheelSubCounter = 1;
                foreach (var wheelStrain in wheelStrains)
                {
                    if (wheelArea == null)
                    {
                        wheelArea = wheelStrain.AxleRef.WheelWidth * wheelStrain.AxleRef.WheelLength;
                    }
                    wheels.Add(GetAnalysisWheel(wheelStrain, leftIntervalStart, wheelCounter, wheelSubCounter));
                    wheelSubCounter++;
                }
                wheelCounter++;
            }

            List<TrafficJamStrainAnalysis>? intervals = null;
            ProfileVector[]? intervalProfileVectors = null; 
            if (columnStrain.TrafficJamStrain != null)
            {
                intervals = new List<TrafficJamStrainAnalysis>();
                var trajectory = columnStrain.VehicleTrajectoryRef;

                var profileLeft = trajectory.Left.Last().Value;
                var profileRight = trajectory.Right.Last().Value;
                var profileCenter = trajectory.Center;
                var leftPieces = profileLeft.PositivePieces;
                var rightPieces = profileRight.PositivePieces;
                var centerPieces = profileCenter.PositivePieces;

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

                    var negativeAreaLeft = MathExtensions.CalculateAreaUnderCurve(profileLeft.SortedVectors);
                    var negativeAreaRight = MathExtensions.CalculateAreaUnderCurve(profileRight.SortedVectors);
                    intervals.Add(new TrafficJamStrainAnalysis
                    {
                        Number = oneBaseColumNumber,
                        LeftIntervalStart = MathExtensions.ToDecimal(left.Start - minusKiller),
                        LeftIntervalEnd = MathExtensions.ToDecimal(left.End - minusKiller),
                        LeftIntervalLength = MathExtensions.ToDecimal(left.Length),
                        LeftIntervalStrain = MathExtensions.ToDecimal(columnStrain.TrafficJamStrain.LeftStrain),
                        RightIntervalStart = MathExtensions.ToDecimal(right.Start - minusKiller),
                        RightIntervalEnd = MathExtensions.ToDecimal(right.End - minusKiller),
                        RightIntervalLength = MathExtensions.ToDecimal(right.Length),
                        RightIntervalStrain = MathExtensions.ToDecimal(columnStrain.TrafficJamStrain.RightStrain),
                        SumStrain = MathExtensions.ToDecimal(columnStrain.TrafficJamStrain.SumStrain),
                        CenterIntervalStart = MathExtensions.ToDecimal(center.Start - minusKiller),
                        CenterIntervalEnd = MathExtensions.ToDecimal(center.End - minusKiller),
                        CenterIntervalLength = MathExtensions.ToDecimal(center.Length),
                        LeftIntervalVolume = MathExtensions.ToDecimal((leftPieces.Sum(x => x.Length) - negativeAreaLeft) * wheelArea ?? 0d),
                        RightIntervalVolume = MathExtensions.ToDecimal((rightPieces.Sum(x => x.Length) - negativeAreaRight) * wheelArea ?? 0d),
                    });
                }

                intervalProfileVectors = GetProfileVectors(trajectory)?.ToArray();
            }

            return new AnalysisVehicle
            {
                ColumnNumber = oneBaseColumNumber,
                VehicleNumber = zeroBaseVehicleNumber + 1,
                Wheels = wheels,
                Intervals = intervals,
                PositionX = MathExtensions.ToDecimal(vehicleStrain.WheelStrains.Average(x => x.Position.X) - leftIntervalStart),
                PositionY = MathExtensions.ToDecimal(vehicleStrain.Position),
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
                Pressure = MathExtensions.ToDecimal(wheelStrain.Strain / wheelStrain.AxleRef.WheelLength / wheelStrain.AxleRef.WheelWidth),

                FootPrintSizeFirst = 0.56m,
                FootPrintSizeSecond = 0.96m,
                ZVolume = MathExtensions.ToDecimal(wheelStrain.Strain / wheelStrain.AxleRef.WheelWeight / 0.56 / 0.96),
            };
        }

        private IEnumerable<ProfileVector>? GetProfileVectors(VehicleTrajectory trajectory)
        {
            var vectors = trajectory.Center.SortedVectors;
            if (vectors.Length == 0)
            {
                return null;
            }
            if (vectors.Length < ProfileVectorsLimitCount)
            {
                return trajectory.Center.SortedVectors.Select<Vector2D, ProfileVector>(x => (MathExtensions.ToDecimal(x.X), MathExtensions.ToDecimal(x.Y)));
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

        /// <summary>
        /// Убираем два идентичных результата, которые возникают в случае с <see cref="StrainCalculationGroupTypeEnum.Pillar"/>
        /// </summary>
        private static void FilterDefaultsForPillar(VehicleRollingResult vehicleRollingResult, List<AnalysisDefault> defaults)
        {
            if (vehicleRollingResult.DataModel.Data.Surface.StrainCalculationGroupType == StrainCalculationGroupTypeEnum.Pillar)
            {
                defaults.RemoveAll(x => x.HasSafetyLine == false);
                foreach (var def in defaults)
                {
                    def.HasSafetyLine = null;
                }
            }
        }
    }
}
