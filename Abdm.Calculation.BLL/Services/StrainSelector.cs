using System.Data;
using System.Linq;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Helpers;
using static System.Formats.Asn1.AsnWriter;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainSelector(IEqualityComparer<double> equalityComparer) : IStrainSelector
    {
        public IList<StrainResultUnpopulated> SelectBestStrainResult(
        StrainMap[] strainMaps,
        VehicleRollingBigModel bigData)
        {
            var data = bigData.Data;
            var roadRules = bigData.RoadRules;
            var stripeCoefficientProvider = bigData.Data.CoefficientProvider;
            var result = new List<StrainResultUnpopulated>();
            foreach (var roadRule in roadRules)
            {
                var strains = strainMaps.Where(m => m.RoadRuleRef == roadRule).ToArray();
                if (strains.Length == 0)
                {
                    continue;
                }

                var actualTrajectoryDistance = Math.Max(roadRule.MinTrajectoryDistance, data.Load.Width + data.Load.Interval);

                var groupedByIntervals = strains
                    .Where(s => s.StrainsInMaximums.Length > 0)
                    .GroupBy(x => x.IntervalModelRef)
                    .Select(x => (x.Single().StrainsInMaximums, Depth: Math.Min(roadRule.MaxTrajectoriesInInterval, x.Key.PassageIntervalRef.LaneCount)))
                    .ToArray();

                var globalDepth = roadRule.MaxTrajectoriesTotal;

                if (groupedByIntervals.All(x => x.Depth <= 1))
                {
                    result.Add(new StrainResultUnpopulated
                    {
                        RoadRuleRef = roadRule,
                        Strain = groupedByIntervals
                        .Select(x => x.StrainsInMaximums.OrderDescending().First())
                        .OrderDescending()
                        .Take(globalDepth)
                        .ToArray()
                    });
                }
                else
                {
                    result.Add(new StrainResultUnpopulated() { 
                        RoadRuleRef = roadRule, 
                        Strain = PickStrains(groupedByIntervals, actualTrajectoryDistance, globalDepth, stripeCoefficientProvider) 
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Выбирает подходящие траектории движения для случая, если двигаются более одного ТС. 
        /// Траектории выбираются исходя из максимального напряжения, но ТС не должны "налезать" друг на друга. 
        /// Но также проверяется оптимальная траектория рядом с уже установленным ТС 
        /// </summary>
        /// <param name="strains">Напряжения c координатой траектории</param>
        /// <param name="actualVehicleCount">Количество ТС</param>
        /// <returns></returns>
        private StrainsInMaximums[] PickStrains(
            (StrainsInMaximums[] strains, int maxDepth)[] intervals,
            double actualTrajectoryDistance,
            int globalDepth,
            ICoefficientProvider stripeCoefficientProvider)
        {
            var orderedByPosition = intervals.Select(i => (i.strains.OrderBy(x => x.X).ToArray(), i.maxDepth)).ToArray();

            var scores = new List<StrainScore>();
            foreach (var interval in orderedByPosition)
            {
                foreach (var strain in interval.Item1)
                {
                    var strainScore = MeasureScore(orderedByPosition, strain, interval, actualTrajectoryDistance, globalDepth, stripeCoefficientProvider);
                    scores.Add(strainScore);
                }
            }

            var finalScore = scores.MaxBy(x => x.TotalScore);

            return finalScore!.StrainsPicked.ToArray();
        }

        private StrainScore MeasureScore(
            (StrainsInMaximums[] orderedByPosition, int depthParent)[] intervals, 
            StrainsInMaximums strainPicked,
            (StrainsInMaximums[], int maxDepth) intervalPicked,
            double actualTrajectoryDistance,
            int globalDepth,
            ICoefficientProvider stripeCoefficientProvider)
        {
            StrainScore? strainScore = null;
            //var validIntervals = intervals.Where(x => x.orderedByPosition.Length > 0 && x.depthParent >= 1).ToArray();
            if (globalDepth >= 2)
            {
                (StrainsInMaximums[] orderedByPosition, int depthParent)[] intervalsForChildArray;
                if (intervalPicked.maxDepth == 1)
                {
                    intervalsForChildArray = intervals.Except([intervalPicked]).ToArray();
                }
                else
                {
                    var interval = intervalPicked.Item1;
                    var leftEdge = Formulas.FindBetweenIndexes(interval, strainPicked.X - actualTrajectoryDistance, (x) => x.X, equalityComparer);
                    var rightEdge = Formulas.FindBetweenIndexes(interval, strainPicked.X + actualTrajectoryDistance, (x) => x.X, equalityComparer);

                    StrainsInMaximums[]? newOrdered = null;
                    if (leftEdge.Left == rightEdge.Right)
                    {
                        newOrdered = interval;
                    }
                    else if (leftEdge.Left != null || rightEdge.Right != null)
                    {
                        var indexLeft = (leftEdge.Left ?? -1) + 1;
                        var indexRight = rightEdge.Right ?? interval.Length;

                        newOrdered = interval.Take(indexLeft).Concat(interval.Skip(indexRight)).ToArray();
                    }

                    var intervalsForChild = intervals.Except([intervalPicked]);

                    if (newOrdered != null && newOrdered.Length > 0)
                    {
                        var newLocalDepth = intervalPicked.maxDepth - 1;

                        intervalsForChild = intervalsForChild.Append((newLocalDepth == 1 ? [newOrdered.MaxBy(x => x.TotalStrain)!] : newOrdered, newLocalDepth));
                    }

                    intervalsForChildArray = intervalsForChild.ToArray();
                }

                var childStrains = new List<StrainScore>();
                foreach (var interval in intervalsForChildArray)
                {
                    foreach (var strain in interval.Item1)
                    {
                        childStrains.Add(MeasureScore(intervalsForChildArray, strain, interval, actualTrajectoryDistance, globalDepth - 1, stripeCoefficientProvider));
                    }
                }

                var strainScoreFromChild = childStrains.MaxBy(score => score.TotalScore);

                if (strainScoreFromChild != null && !(strainScore?.TotalScore > strainScoreFromChild.TotalScore))
                {
                    strainScore = strainScoreFromChild;
                }
            }
            
            if (strainScore == null || strainScore.Score < 0)
            {
                strainScore = new StrainScore { Score = 0, StrainsPicked = new List<StrainsInMaximums>() };
            }

            var coefficients = stripeCoefficientProvider.GetStripeCoefficient(strainPicked.Strains.First().LambdaSmall);
            var coefficientToPick = Math.Min(4, strainScore.StrainsPicked.Count);

            strainScore.Score += strainPicked.TotalStrain;
            strainScore.TotalScore = strainPicked.TotalStrain * coefficients[coefficientToPick];
            strainScore.StrainsPicked.Add(strainPicked);

            return strainScore;
        }
    }
}
