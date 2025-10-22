using System.Data;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCalculator(IVehicleTrajectoryService vehicleTrajectoryService, ITrajectorySelector trajectorySelector) : IStrainCalculator
    {
        public IEnumerable<StrainResult> GetStrainResultFromTrajectories(
        Dictionary<RoadRule, (double X, double Strain)[]> orderedTrajectoriesMap,
        IntervalModel intervalModel,
        IEnumerable<RoadRule> roadRules,
        PassTypeSmallModel data,
        Mesh mesh)
        {
            foreach (var roadRule in roadRules)
            {
                var actualVehicleCount = Math.Min(roadRule.MaxVehicleCount, intervalModel.PassageIntervalRef.LaneCount);
                
                if (actualVehicleCount == 1)
                {
                    yield return
                        new StrainResult
                        {
                            RoadRuleRef = roadRule,
                            Strain = orderedTrajectoriesMap[roadRule].First().Strain,
                            StrainOneAuto = orderedTrajectoriesMap[roadRule].First().Strain
                        };
                }
                else
                {
                    yield return GetStrainResult(orderedTrajectoriesMap[roadRule], intervalModel, roadRule, data, mesh, actualVehicleCount);
                }
            }
        }

        private StrainResult GetStrainResult((double X, double Strain)[] sortedTrajStrains,
            IntervalModel intervalModel,
            RoadRule roadRule,
            PassTypeSmallModel data,
            Mesh mesh,
            int actualVehicleCount)
        {
            var strainResult = new StrainResult
            {
                RoadRuleRef = roadRule,
                Strain = 0d,
                StrainOneAuto = Double.NaN
            };

            var trajectoriesCanUse = sortedTrajStrains.Select(x => x.X).ToHashSet();
            var sortedAdditionalTrajectories = new List<(double X, double Strain)>();

            for (var i = 0; i < actualVehicleCount; i++)
            {
                if (trajectoriesCanUse.Count > 0)
                {
                    break;
                }
                (double X, double Strain)? maxTrajectoryOriginal
                    = sortedTrajStrains.FirstOrDefault(x => trajectoriesCanUse.Contains(x.X));
                (double X, double Strain)? maxTrajectoryAdditional
                    = sortedAdditionalTrajectories.FirstOrDefault(x => trajectoriesCanUse.Contains(x.X));

                if ((maxTrajectoryOriginal?.Strain ?? 0d) >= (maxTrajectoryAdditional?.Strain ?? 0d))
                {
                    UseTrajectory(maxTrajectoryOriginal);
                }
                else
                {
                    UseTrajectory(maxTrajectoryAdditional);
                }
            }

            return strainResult;

            void UseTrajectory((double X, double Strain)? trajNullable)
            {
                if (trajNullable is not (double X, double Strain) traj)
                {
                    return;
                }
                strainResult.Strain += traj.Strain;
                if (Double.IsNaN(strainResult.StrainOneAuto))
                {
                    strainResult.StrainOneAuto = traj.Strain;
                }
                var left = traj.X - roadRule.MinTrajectoryDistance - data.Load.Width;
                var right = traj.X + roadRule.MinTrajectoryDistance + data.Load.Width;
                trajectoriesCanUse.RemoveWhere(t => left < t && t < right);

                TryAddTrajectory(left);
                TryAddTrajectory(right);
            }

            void TryAddTrajectory(double traj)
            {
                if (!trajectoriesCanUse.Contains(traj)
                            && intervalModel.PassageIntervalRef.AbsolutePositionLeft <= traj
                            && traj <= intervalModel.PassageIntervalRef.AbsolutePositionRight)
                {
                    if (vehicleTrajectoryService.GetVehicleTrajectory(mesh, data.Load, traj) is VehicleTrajectory additionalTrajectory)
                    {
                        var additionalTrajectoryStrain = trajectorySelector.GetStrainForEachPositivePiece(additionalTrajectory, data, roadRule.DoTrafficJamLoadCalulation).Max();
                        sortedAdditionalTrajectories = sortedAdditionalTrajectories.Append((traj, additionalTrajectoryStrain)).OrderByDescending(x => x.Item2).ToList();
                        trajectoriesCanUse.Add(traj);
                    }
                }
            }
        }
    }
}
