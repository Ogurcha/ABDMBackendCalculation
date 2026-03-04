using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Models.StrainAnalysis;
using Abdm.Calculation.BLL.Models.StrainAnalysis.Pillar;
using Abdm.Calculation.Maths.Extensions;

namespace Abdm.Calculation.BLL.Services.StrainAnlysis.Strategies
{
    public class PillarSAStrategy : ISAStrategy
    {
        public StrainCalculationGroupTypeEnum[] StrainCalculationGroupTypes { get => [
            StrainCalculationGroupTypeEnum.Pillar
        ];}

        public AnalysisSummary Analyse(AnalysisSummary analysis, VehicleRollingResult vehicleRollingResult)
        {
            var strainResult = vehicleRollingResult.StrainResults.OrderBy(x => x.Strain.TotalStrain).Last();
            var pillars = new List<AnalysisPillar>();
            var columnCounter = 1;
            foreach (var strain in strainResult.Strain.OrderBy(x => x.WheelStrains.Min(w => w.Position.X)))
            {
                pillars.Add(GetAnalysisPillar(strain, vehicleRollingResult.DataModel.Intervals.First().AbsolutePositionLeft, columnCounter));
                columnCounter++;
            }

            analysis.Pillar = pillars;
            return analysis;
        }

        private AnalysisPillar GetAnalysisPillar(VehicleStrain vehicleStrain, double leftIntervalStart, int columNumber)
        {
            var wheelCounter = 1;
            var wheels = new List<AxleAnalysis>();
            foreach (var wheelStrains in vehicleStrain.WheelStrains.OrderBy(x => x.Position.Y).GroupBy(x => x.Position.Y))
            {
                wheels.Add(GetAnalysisAxles(wheelStrains, leftIntervalStart, wheelCounter));
                wheelCounter++;
            }

            List<TrafficJamStrainAnalysisSlim>? intervals = null;
            if (vehicleStrain.TrafficJamStrain != null)
            {
                intervals = new List<TrafficJamStrainAnalysisSlim>();

                for (var i = 0; i < vehicleStrain.TrafficJamStrain.LeftPieces.Length; i++)
                {
                    var left = vehicleStrain.TrafficJamStrain.LeftPieces[i];

                    var leftLength = left.EndY - left.BeginY;

                    intervals.Add(new TrafficJamStrainAnalysisSlim
                    {
                        Number = columNumber,
                        IntervalStart = MathExtensions.ToDecimal(left.BeginY),
                        IntervalEnd = MathExtensions.ToDecimal(left.EndY),
                        IntervalLength = MathExtensions.ToDecimal(leftLength),
                        SumStrain = MathExtensions.ToDecimal(vehicleStrain.TrafficJamStrain.SumStrain)
                    });
                }
            }

            return new AnalysisPillar
            {
                ColumnNumber = columNumber,
                Axles = wheels,
                Intervals = intervals,
            };
        }

        private AxleAnalysis GetAnalysisAxles(IGrouping<double, WheelStrain> wheelStrains, double leftIntervalStart, int number)
        {
            return new AxleAnalysis()
            {
                Number = number,
                Strain = MathExtensions.ToDecimal(wheelStrains.Sum(x => x.Strain)),
                PositionY = MathExtensions.ToDecimal(wheelStrains.Key),
                Z = MathExtensions.ToDecimal(wheelStrains.Average(x => x.Strain) / wheelStrains.First().AxleRef.Weight),
                Weight = MathExtensions.ToDecimal(wheelStrains.First().AxleRef.Weight),
            };
        }
    }
}
