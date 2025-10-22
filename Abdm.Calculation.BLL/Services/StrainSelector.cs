using System.Data;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainSelector(IVehicleTrajectoryService vehicleTrajectoryService, IStrainCalculator strainCalculator) : IStrainSelector
    {
        public IEnumerable<StrainResult> GetStrainResults(
        Dictionary<RoadRule, (double X, double Strain)[]> orderedStrainsMap,
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
                            Strain = orderedStrainsMap[roadRule].First().Strain,
                            StrainOneAuto = orderedStrainsMap[roadRule].First().Strain
                        };
                }
                else
                {
                    yield return GetStrainResult(orderedStrainsMap[roadRule], intervalModel, roadRule, data, mesh, actualVehicleCount);
                }
            }
        }

        private StrainResult GetStrainResult((double X, double Strain)[] sortedStrains,
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

            var strainsCanUse = sortedStrains.Select(x => x.X).ToHashSet();
            var sortedAdditionalStrains = new List<(double X, double Strain)>();

            for (var i = 0; i < actualVehicleCount; i++)
            {
                if (strainsCanUse.Count > 0)
                {
                    break;
                }
                (double X, double Strain)? maxStrainOriginal
                    = sortedStrains.FirstOrDefault(x => strainsCanUse.Contains(x.X));
                (double X, double Strain)? maxStrainAdditional
                    = sortedAdditionalStrains.FirstOrDefault(x => strainsCanUse.Contains(x.X));

                if ((maxStrainOriginal?.Strain ?? 0d) >= (maxStrainAdditional?.Strain ?? 0d))
                {
                    UseStrain(maxStrainOriginal);
                }
                else
                {
                    UseStrain(maxStrainAdditional);
                }
            }

            return strainResult;

            void UseStrain((double X, double Strain)? trajNullable)
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
                strainsCanUse.RemoveWhere(t => left < t && t < right);

                TryAddTrajectory(left);
                TryAddTrajectory(right);
            }

            void TryAddTrajectory(double traj)
            {
                if (!strainsCanUse.Contains(traj)
                            && intervalModel.PassageIntervalRef.AbsolutePositionLeft <= traj
                            && traj <= intervalModel.PassageIntervalRef.AbsolutePositionRight)
                {
                    if (vehicleTrajectoryService.GetVehicleTrajectory(mesh, data.Load, traj) is VehicleTrajectory additionalTrajectory)
                    {
                        var additionalTrajectoryStrain = strainCalculator.GetStrainForEachPositivePiece(additionalTrajectory, data, roadRule.DoTrafficJamLoadCalulation).Max();
                        sortedAdditionalStrains = sortedAdditionalStrains.Append((traj, additionalTrajectoryStrain)).OrderByDescending(x => x.Item2).ToList();
                        strainsCanUse.Add(traj);
                    }
                }
            }
        }
    }
}
