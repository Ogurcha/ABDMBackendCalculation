using System.Data;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Helpers;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainSelector(IEqualityComparer<double> equalityComparer) : IStrainSelector
    {
        public IEnumerable<StrainResultUnpopulated> SelectBestStrainResult(
        Dictionary<RoadRule, StrainsInMaximums[]> strainsMap,
        IntervalModel intervalModel,
        VehicleRollingBigModel bigData)
        {
            var data = bigData.Data;
            var roadRules = bigData.RoadRules;
            foreach (var roadRule in roadRules)
            {
                var strains = strainsMap[roadRule];
                if (strains.Length == 0)
                {
                    continue;
                }

                var actualVehicleCount = Math.Min(roadRule.MaxTrajectoriesCount, intervalModel.PassageIntervalRef.LaneCount);
                
                if (actualVehicleCount == 1)
                {
                    yield return
                        new StrainResultUnpopulated
                        {
                            RoadRuleRef = roadRule,
                            Strain = [strains.OrderDescending().First()],
                            IntervalModelRef = intervalModel
                        };
                }
                else
                {
                    //TODO#1: Минорная проблема, которая пока что не актуальна. В случае, если в метод попадут несколько roadRules,
                    //то даже если у них будет одинаковый actualVehicleCount+MinTrajectoryDistance, то цикл будет вызываться несколько раз. Чтобы избежать этого, нужно сделать группировку, как в StrainResultPopulator, VehicleTrajectoryFilter, StrainCalculator. Но это пока что не актуально, так как не встретился пока что реальный снип, который содержит пару roadRules с одинаковым actualVehicleCount+MinTrajectoryDistance
                    var strainResult = GetStrainResult(strains, intervalModel, roadRule, actualVehicleCount);

                    if (strainResult == null) 
                    { 
                        continue; 
                    }

                    yield return strainResult!;
                }
            }
        }

        /// <summary>
        /// Выбирает подходящие траектории движения для случая, если двигаются более одного ТС. 
        /// Траектории выбираются исходя из максимального напряжения, но ТС не должны "налезать" друг на друга. 
        /// Но также проверяется оптимальная траектория рядом с уже установленным ТС 
        /// </summary>
        /// <param name="strains">Напряжения c координатой траектории</param>
        /// <param name="intervalModel">интервал моста, внутри которого происходит движение</param>
        /// <param name="roadRule">Правила движения по мосту</param>
        /// <param name="actualVehicleCount">Количество ТС</param>
        /// <returns></returns>
        private StrainResultUnpopulated? GetStrainResult(StrainsInMaximums[] strains,
            IntervalModel intervalModel,
            RoadRule roadRule,
            int actualVehicleCount)
        {
            var orderedByPosition = strains.OrderBy(x => x.X).ToArray();

            var scores = new List<StrainScore>();
            var depth = actualVehicleCount;
            foreach (var strain in strains)
            {
                var strainScore = MeasureScore(orderedByPosition, strain, depth, roadRule);
                scores.Add(strainScore);
            }

            var finalScore = scores.OrderBy(x => x.Score).Last();

            var strainResult = new StrainResultUnpopulated
            {
                RoadRuleRef = roadRule,
                Strain = finalScore.StrainsPicked.ToArray(),
                IntervalModelRef = intervalModel,
            };

            return strainResult;
        }

        private StrainScore MeasureScore(
            StrainsInMaximums[] orderedByPosition,
            StrainsInMaximums strainPicked, 
            int depthParent,
            RoadRule roadRule)
        {
            StrainScore? strainScore = null;
            if (orderedByPosition.Length > 0 && depthParent >= 2)
            {
                var leftEdge = Formulas.FindBetweenIndexes(orderedByPosition, strainPicked.X - roadRule.MinTrajectoryDistance, (x) => x.X, equalityComparer);
                var rightEdge = Formulas.FindBetweenIndexes(orderedByPosition, strainPicked.X + roadRule.MinTrajectoryDistance, (x) => x.X, equalityComparer);

                StrainsInMaximums[] newOrdered;
                if (leftEdge.Left == rightEdge.Right)
                {
                    newOrdered = orderedByPosition;
                }
                else if (leftEdge.Left != null || rightEdge.Right != null)
                {
                    var indexLeft = (leftEdge.Left ?? -1) + 1;
                    var indexRight = rightEdge.Right ?? orderedByPosition.Length;

                    newOrdered = orderedByPosition.Take(indexLeft).Concat(orderedByPosition.Skip(indexRight)).ToArray();

                    strainScore = newOrdered.Select(strain => MeasureScore(newOrdered, strain, depthParent - 1, roadRule)).OrderBy(score => score.Score).LastOrDefault();
                }
            }

            if (strainScore == null || strainScore.Score < 0)
            {
                strainScore = new StrainScore { Score = 0, StrainsPicked = new List<StrainsInMaximums>() };
            }

            strainScore.Score += strainPicked.TotalStrain;
            strainScore.StrainsPicked.Add(strainPicked);

            return strainScore;
        }
    }
}
