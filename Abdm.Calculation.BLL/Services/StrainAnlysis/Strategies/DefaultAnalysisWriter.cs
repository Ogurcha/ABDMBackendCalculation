using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Default;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis.Strategies
{
    public class DefaultAnalysisWriter() : IAnalysisWriter
    {
        private const int ProfileVectorsLimitCount = 50;
        private const int DecimalPrecision = 2;
        private readonly double DecimalPrecisionValue = Math.Pow(10, -DecimalPrecision);

        public virtual StrainCalculationGroupTypeEnum[] StrainCalculationGroupTypes { get => [
            StrainCalculationGroupTypeEnum.Default,
            StrainCalculationGroupTypeEnum.Slab,
            StrainCalculationGroupTypeEnum.Pillar,
        ];}

        public virtual AnalysisSummary Analyse(AnalysisSummary analysis, 
            VehicleRollingResult vehicleRollingResult,
            VehicleRollingResult? rollingResultBackWardsNullable,
            bool doNegativeNumbers)
        {
            var defaults = new List<AnalysisDefault>();
            var dataModel = vehicleRollingResult.DataModel;

            foreach (var strainResults in vehicleRollingResult.StrainResults)
            {
                var strains = strainResults.VehicleColumnStrains;

                if (strains.Length == 0)
                {
                    continue;
                }

                defaults.Add(new AnalysisDefault
                {
                    HasSafetyLine = strainResults.RoadRuleRef.HasSafetyLine,
                    Columns = GetAnalysisColumns(strains, dataModel).ToArray(),
                    IsForward = true
                });
            }
            if (rollingResultBackWardsNullable is VehicleRollingResult VehicleRollingResult)
            {
                foreach (var strainResults in rollingResultBackWardsNullable.StrainResults)
                {
                    var strains = strainResults.VehicleColumnStrains;

                    if (strains.Length == 0)
                    {
                        continue;
                    }

                    defaults.Add(new AnalysisDefault
                    {
                        HasSafetyLine = strainResults.RoadRuleRef.HasSafetyLine,
                        Columns = GetAnalysisColumns(strains, dataModel).ToArray(),
                        IsForward = false
                    });
                }
            }
            FilterDefaultsForPillar(vehicleRollingResult, defaults);

            analysis.Lambda = ToDecimal(vehicleRollingResult.DataModel.Data.Surface.Lambda);
            analysis.MyStrength = ToDecimal(vehicleRollingResult.DataModel.Data.Surface.MyStrength);
            analysis.ConstLoad = ToDecimal(vehicleRollingResult.DataModel.Data.Surface.ConstLoad);
            analysis.PedestrianLoad = ToDecimal(vehicleRollingResult.DataModel.Data.Surface.PedestrianLoad);
            analysis.OtherLoad = ToDecimal(vehicleRollingResult.DataModel.Data.Surface.OtherLoad);
            analysis.Default = defaults.OrderByDescending(x => x.HasSafetyLine).OrderByDescending(x => x.IsForward).ToList();

            if (doNegativeNumbers)
            {
                InvertSummary(analysis);
            }

            var dynamicCoefficient = dataModel.Data.DynamicCoefficient();
            foreach (var column in analysis.Default.SelectMany(x => x.Columns))
            {
                column.Coefficients.Dynamic = ToDecimal(dynamicCoefficient);
                column.TotalStrain = decimal.Round(column.SumStrain * column.Coefficients.Dynamic, 2);
                if (column.Intervals != null)
                {
                    column.Coefficients.DynamicInterval = column.Coefficients.Dynamic;
                }
            }

            return analysis;
        }

        private void InvertSummary(AnalysisSummary summary)
        {
            foreach (var analysis in summary.Default!)
            {
                foreach (var column in analysis.Columns)
                {
                    column.SumStrain = -column.SumStrain;
                    column.TotalStrain = -column.TotalStrain;
                    foreach (var v in column.IntervalProfileVectors)
                    {
                        v.Y = -v.Y;
                    }
                    foreach (var w in column.Wheels)
                    {
                        w.Strain = -w.Strain;
                        w.Z = -w.Z;
                        w.ZVolume = -w.ZVolume;
                    }
                    if (column.Intervals != null)
                    {
                        foreach (var i in column.Intervals)
                        {
                            i.LeftIntervalVolume = -i.LeftIntervalVolume;
                            i.RightIntervalVolume = -i.RightIntervalVolume;
                            i.RightIntervalStrain = -i.RightIntervalStrain;
                            i.LeftIntervalStrain = -i.LeftIntervalStrain;
                            i.SumStrain = -i.SumStrain;
                        }
                    }
                }
            }
        }

        private List<AnalysisColumn> GetAnalysisColumns(IEnumerable<VehicleColumnStrain> strainResults, 
            VehicleRollingBigModel data)
        {
            var vehicles = new List<AnalysisColumn>();
            var columnCounter = 1;
            foreach (var columnStrains in strainResults.Where(x => x.TotalStrain > DecimalPrecisionValue).OrderBy(x => x.VehicleTrajectoryRef.X))
            {
                vehicles.AddRange(GetAnalysisColumn(columnStrains, data, columnCounter));
                columnCounter++;
            }

            return vehicles;
        }

        private List<AnalysisColumn> GetAnalysisColumn(VehicleColumnStrain columnStrain, 
            VehicleRollingBigModel data, 
            int oneBaseColumNumber)
        {
            var vehicles = new List<AnalysisColumn>();
            for (int vehicleCounter = 0; vehicleCounter < columnStrain.VehicleStrains.Length; vehicleCounter++)
            {
                var vehicle = GetAnalysisVehicle(columnStrain,
                    data.Intervals.First().AbsolutePositionLeft,
                    oneBaseColumNumber,
                    vehicleCounter);
                if (vehicle != null)
                {
                    vehicles.Add(vehicle);
                }
            }

            return vehicles;
        }

        private AnalysisColumn? GetAnalysisVehicle(VehicleColumnStrain columnStrain,
            double xPositionShift,
            int oneBaseColumNumber,
            int zeroBaseVehicleNumber)
        {
            var wheelCounter = 1;
            var wheels = new List<WheelAnalysis>();
            var vehicleStrain = columnStrain.VehicleStrains[zeroBaseVehicleNumber];
            var trajectory = columnStrain.VehicleTrajectoryRef;
            if (vehicleStrain == null)
            {
                return null;
            }

            foreach (var wheelStrains in vehicleStrain.WheelStrains.OrderBy(x => x.Position.Y).GroupBy(x => x.Position.Y))
            {
                var wheelSubCounter = 1;
                foreach (var wheelStrain in wheelStrains)
                {
                    wheels.Add(GetAnalysisWheel(wheelStrain, xPositionShift, wheelCounter, wheelSubCounter));
                    wheelSubCounter++;
                }
                wheelCounter++;
            }

            List<TrafficJamStrainAnalysis>? intervals = null;
            if (columnStrain.TrafficJamStrain != null)
            {
                intervals = new List<TrafficJamStrainAnalysis>();
                
                foreach (var strainPiece in columnStrain.TrafficJamStrain.StrainPieces.Where(x => x.LeftStrain + x.RightStrain > DecimalPrecisionValue))
                {
                    var leftIntervalStart = Math.Max(columnStrain.VehicleTrajectoryRef.Left.Last().Value.SortedVectors[1].X, strainPiece.Interval.Start);
                    var leftIntervalEnd = Math.Min(columnStrain.VehicleTrajectoryRef.Left.Last().Value.SortedVectors[^2].X, strainPiece.Interval.End);
                    var rightIntervalStart = Math.Max(columnStrain.VehicleTrajectoryRef.Right.Last().Value.SortedVectors[1].X, strainPiece.Interval.Start);
                    var rightIntervalEnd = Math.Min(columnStrain.VehicleTrajectoryRef.Right.Last().Value.SortedVectors[^2].X, strainPiece.Interval.End);

                    intervals.Add(new TrafficJamStrainAnalysis
                    {
                        Number = oneBaseColumNumber,
                        LeftIntervalStart = ToDecimal(leftIntervalStart),
                        LeftIntervalEnd = ToDecimal(leftIntervalEnd),
                        LeftIntervalLength = ToDecimal(leftIntervalEnd - leftIntervalStart),
                        LeftIntervalVolume = decimal.Round((decimal)strainPiece.LeftVolume, 4),
                        LeftIntervalIntensity = strainPiece.LeftVolume == 0 ? 0 : ToDecimal(strainPiece.LeftStrain / strainPiece.LeftVolume),
                        LeftIntervalStrain = ToDecimal(strainPiece.LeftStrain),
                        RightIntervalStart = ToDecimal(rightIntervalStart),
                        RightIntervalEnd = ToDecimal(rightIntervalEnd),
                        RightIntervalLength = ToDecimal(rightIntervalEnd - rightIntervalStart),
                        RightIntervalVolume = decimal.Round((decimal)strainPiece.RightVolume, 4),
                        RightIntervalIntensity = strainPiece.RightVolume == 0 ? 0 : ToDecimal(strainPiece.RightStrain / strainPiece.RightVolume),
                        RightIntervalStrain = ToDecimal(strainPiece.RightStrain),
                        SumStrain = ToDecimal(strainPiece.LeftStrain + strainPiece.RightStrain),
                        CenterIntervalStart = ToDecimal(strainPiece.Interval.Start),
                        CenterIntervalEnd = ToDecimal(strainPiece.Interval.End),
                        CenterIntervalLength = ToDecimal(strainPiece.Interval.Length),
                    });
                }
            }

            var yShift = vehicleStrain.WheelStrains.Min(w => w.AxleRef.Position);
            var intervalProfileVectors = GetProfileVectors(trajectory)?.ToArray() ?? [];

            var coefficients = new Coefficients
            {
                Stripe = ToDecimal(columnStrain.StripeCoefficient),
                Reliability = ToDecimal(vehicleStrain.ReliabilityCoefficient),
            };
            if (columnStrain.TrafficJamStrain != null)
            {
                coefficients.StripeInterval = ToDecimal(columnStrain.TrafficJamStripeCoefficient!.Value);
                coefficients.ReliabilityInterval = ToDecimal(columnStrain.TrafficJamStrain.ReliabilityCoefficient);
            }

            return new AnalysisColumn
            {
                ColumnNumber = oneBaseColumNumber,
                VehicleNumber = zeroBaseVehicleNumber + 1,
                Wheels = wheels,
                Intervals = intervals,
                PositionX = ToDecimal(vehicleStrain.X - xPositionShift),
                PositionY = ToDecimal(vehicleStrain.Y + yShift),
                PositionYForImage = ToDecimal(vehicleStrain.Y + yShift),
                SumStrain = wheels.Sum(w => w.Strain),
                IntervalProfileVectors = intervalProfileVectors,
                LambdaSmall = ToDecimal(vehicleStrain.LambdaSmall),
                Coefficients = coefficients,
            };
        }

        private WheelAnalysis GetAnalysisWheel(WheelStrain wheelStrain, 
            double xPositionShift, 
            int number, 
            int subNumber)
        {
            return new WheelAnalysis()
            {
                Number = number,
                SubNumber = subNumber,
                Strain = ToDecimal(wheelStrain.Strain),
                PositionX = ToDecimal(wheelStrain.Position.X - xPositionShift),
                PositionY = ToDecimal(wheelStrain.Position.Y),
                Z = ToDecimal(wheelStrain.ZValue),
                ZVolume = decimal.Round((decimal)(wheelStrain.ZValue * wheelStrain.FootprintLength * wheelStrain.FootprintWidth ?? 0), 4),
                Weight = ToDecimal(wheelStrain.AxleRef.WheelWeight),
                Pressure = ToDecimal(wheelStrain.AxleRef.WheelWeight / wheelStrain.FootprintLength / wheelStrain.FootprintWidth ?? 0),
                FootPrintSizeFirst = ToDecimal(wheelStrain.FootprintLength ?? 0d),
                FootPrintSizeSecond = ToDecimal(wheelStrain.FootprintWidth ?? 0d),
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
                return trajectory.Center.SortedVectors.Select<Vector2D, ProfileVector>(x => (ToDecimal(x.X), ToDecimal(x.Y)));
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
                        yield return (ToDecimal(vector.X), ToDecimal(vector.Y));
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

        private decimal ToDecimal(double value) => decimal.Round((decimal)value, DecimalPrecision);
    }
}
