using System.Data;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Helpers;

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
                    .GroupBy(x => x.IntervalModelRef)
                    .Select(x => (x.Single().StrainsInMaximums, Math.Min(roadRule.MaxTrajectoriesInInterval, x.Key.PassageIntervalRef.LaneCount)))
                    .ToArray();

                var globalDepth = roadRule.MaxTrajectoriesTotal;

                if (groupedByIntervals.All(x => x.Item2 <= 1))
                {
                    result.Add(new StrainResultUnpopulated
                    {
                        RoadRuleRef = roadRule,
                        Strain = groupedByIntervals
                        .Select(x => x.Item1.OrderDescending().First())
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
            foreach (var strain in orderedByPosition.SelectMany(x => x.Item1))
            {
                var strainScore = MeasureScore(orderedByPosition, strain, actualTrajectoryDistance, globalDepth, stripeCoefficientProvider);
                scores.Add(strainScore);
            }

            var finalScore = scores.OrderBy(x => x.TotalScore).Last();

            return finalScore.StrainsPicked.ToArray();
        }

        private StrainScore MeasureScore(
            (StrainsInMaximums[] orderedByPosition, int depthParent)[] intervals, 
            StrainsInMaximums strainPicked,
            double actualTrajectoryDistance,
            int globalDepth,
            ICoefficientProvider stripeCoefficientProvider)
        {
            StrainScore? strainScore = null;
            var validIntervals = intervals.Where(x => x.orderedByPosition.Length > 0 && x.depthParent >= 2).ToArray();
            if (globalDepth >= 2)
            {
                for (int i = 0; i < validIntervals.Length; i++)
                {
                    var orderedByPosition = validIntervals[i].orderedByPosition;

                    var leftEdge = Formulas.FindBetweenIndexes(orderedByPosition, strainPicked.X - actualTrajectoryDistance, (x) => x.X, equalityComparer);
                    var rightEdge = Formulas.FindBetweenIndexes(orderedByPosition, strainPicked.X + actualTrajectoryDistance, (x) => x.X, equalityComparer);

                    StrainsInMaximums[]? newOrdered = null;
                    if (leftEdge.Left == rightEdge.Right)
                    {
                        newOrdered = orderedByPosition;
                    }
                    else if (leftEdge.Left != null || rightEdge.Right != null)
                    {
                        var indexLeft = (leftEdge.Left ?? -1) + 1;
                        var indexRight = rightEdge.Right ?? orderedByPosition.Length;

                        newOrdered = orderedByPosition.Take(indexLeft).Concat(orderedByPosition.Skip(indexRight)).ToArray();
                    }

                    if (newOrdered != null)
                    {
                        var newIntervals = validIntervals.Except([validIntervals.ElementAt(i)]).Append((newOrdered, validIntervals[i].depthParent - 1)).ToArray();

                        strainScore = newOrdered.Select(strain => MeasureScore(newIntervals, strain, actualTrajectoryDistance, globalDepth - 1, stripeCoefficientProvider)).OrderBy(score => score.TotalScore).LastOrDefault();
                    }
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
