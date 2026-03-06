using System.Data;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainSelector(
        IVehicleTrajectoryService vehicleTrajectoryService, 
        IStrainCalculator strainCalculator, 
        IEqualityComparer<double> equalityComparer,
        ITrajectoryFilterProvider trajectoryFilterProvider) : IStrainSelector
    {
        public IEnumerable<StrainResult> GetStrainResults(
        Dictionary<RoadRule, (double X, VehicleStrain Strain)[]> orderedStrainsMap,
        IntervalModel intervalModel,
        VehicleRollingBigModel bigData)
        {
            var data = bigData.Data;
            var mesh = bigData.Mesh;
            var roadRules = bigData.RoadRules;
            foreach (var roadRule in roadRules)
            {
                if (!orderedStrainsMap[roadRule].Any())
                {
                    continue;
                }

                var actualVehicleCount = Math.Min(roadRule.MaxTrajectoriesCount, intervalModel.PassageIntervalRef.LaneCount);
                
                if (actualVehicleCount == 1)
                {
                    yield return
                        new StrainResult
                        {
                            RoadRuleRef = roadRule,
                            Strain = [orderedStrainsMap[roadRule].First().Strain],
                            StrainOneAuto = orderedStrainsMap[roadRule].First().Strain
                        };
                }
                else
                {
                    var strainResult = GetStrainResult(orderedStrainsMap[roadRule], intervalModel, roadRule, data, mesh, actualVehicleCount);
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
        /// <param name="sortedStrains">Напряжения отсортированные по убыванию с координатой траектории</param>
        /// <param name="intervalModel">интервал моста, внутри которого происходит движение</param>
        /// <param name="roadRule">Правила движения по мосту</param>
        /// <param name="data">Параметры поверхности и нагрузки</param>
        /// <param name="mesh">Поверхность влияния</param>
        /// <param name="actualVehicleCount">Количество ТС</param>
        /// <returns></returns>
        private StrainResult? GetStrainResult((double X, VehicleStrain Strain)[] sortedStrains,
            IntervalModel intervalModel,
            RoadRule roadRule,
            VehicleRollingSmallModel data,
            Mesh mesh,
            int actualVehicleCount)
        {
            var strainsCanUse = sortedStrains.Select(x => x.X).ToHashSet(equalityComparer);
            var sortedAdditionalStrains = new List<(double X, VehicleStrain Strain)>();

            VehicleStrainList vehicleStrains = new VehicleStrainList();
            VehicleStrain? vehicleStrain = null;

            var trajectoryFilter = trajectoryFilterProvider.GetFilter(intervalModel.PassageIntervalRef, data.Load, roadRule);

            for (var i = 0; i < actualVehicleCount; i++)
            {
                if (strainsCanUse.Count <= 0)
                {
                    break;
                }
                (double X, VehicleStrain Strain)? maxStrainOriginal
                    = sortedStrains.FirstOrDefault(x => strainsCanUse.Contains(x.X));
                (double X, VehicleStrain Strain)? maxStrainAdditional
                    = sortedAdditionalStrains.FirstOrDefault(x => strainsCanUse.Contains(x.X));

                if ((maxStrainOriginal?.Strain?.TotalStrain ?? 0d) >= (maxStrainAdditional?.Strain?.TotalStrain ?? 0d))
                {
                    UseStrain(maxStrainOriginal);
                }
                else
                {
                    UseStrain(maxStrainAdditional);
                }
            }

            if (vehicleStrain == null || vehicleStrains.Count == 0)
            {
                return null;
            }
            else
            {
                var strainResult = new StrainResult
                {
                    RoadRuleRef = roadRule,
                    Strain = vehicleStrains,
                    StrainOneAuto = vehicleStrain
                };

                return strainResult;
            }

            void UseStrain((double X, VehicleStrain Strain)? trajNullable)
            {
                if (trajNullable is not (double X, VehicleStrain Strain) traj)
                {
                    return;
                }
                vehicleStrains.Add(traj.Strain);
                if (vehicleStrain == null)
                {
                    vehicleStrain = traj.Strain;
                }
                var left = traj.X - Math.Max(roadRule.MinTrajectoryDistance, data.Load.Interval);
                var right = traj.X + Math.Max(roadRule.MinTrajectoryDistance, data.Load.Interval);
                strainsCanUse.RemoveWhere(t => left < t && t < right && !equalityComparer.Equals(left, t) && !equalityComparer.Equals(t, right));

                TryAddTrajectory(left);
                TryAddTrajectory(right);
            }

            void TryAddTrajectory(double traj)
            {
                if (!strainsCanUse.Contains(traj)
                    && trajectoryFilter.Filter(traj)
                    && !sortedStrains.Select(s => s.X).Contains(traj, equalityComparer)
                    && !sortedAdditionalStrains.Select(s => s.X).Contains(traj, equalityComparer)
                    && vehicleTrajectoryService.GetVehicleTrajectory(mesh, data.Load, traj) is VehicleTrajectory additionalTrajectory
                    && strainCalculator.GetStrainForEachPositivePiece(additionalTrajectory, data).Max() is VehicleStrain additionalTrajectoryStrain)
                {
                    additionalTrajectoryStrain.TrafficJamStrain = roadRule.DoTrafficJamLoadCalulation ? strainCalculator.GetTrafficJamStrain(additionalTrajectory, data) : null;
                    sortedAdditionalStrains = sortedAdditionalStrains.Append((traj, additionalTrajectoryStrain)).OrderByDescending(x => x.Item2).ToList();
                    strainsCanUse.Add(traj);
                }
            }
        }
    }
}
