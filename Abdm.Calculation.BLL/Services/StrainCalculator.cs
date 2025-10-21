using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCalculator(IVehicleTrajectoryService vehicleTrajectoryService, ITrajectorySelector trajectorySelector) : IStrainCalculator
    {
        public IEnumerable<StrainResult> Calculate(
        Dictionary<RoadRule, (double X, double Strain)[]> orderedTrajectoriesMap,
        IntervalModel intervalModel,
        IEnumerable<RoadRule> roadRules,
        PassTypeSmallModel data,
        Mesh mesh)
        {

            foreach (var roadRule in roadRules)
            {
                var actualVehicleCount = Math.Min(roadRule.MaxVehicleCount, intervalModel.PassageIntervalRef.LaneCount);
                var strain = 0d;
                var strainOneVehicle = Double.NaN;

                if (actualVehicleCount == 1)
                {
                    strain = strainOneVehicle = orderedTrajectoriesMap[roadRule].First().Strain;
                    continue;
                }
                var trajectoriesLeft = orderedTrajectoriesMap[roadRule].Select(x => x.X).ToHashSet();
                var additionalTrajectories = new List<(double X, double Strain)>();
                for (var i = 0; i < actualVehicleCount; i++) { 
                    if (trajectoriesLeft.Count == 0)
                    {
                        break;
                    }
                    (double X, double Strain)? maxTrajectoryOriginal 
                        = orderedTrajectoriesMap[roadRule].FirstOrDefault(x => trajectoriesLeft.Contains(x.X));
                    (double X, double Strain)? maxTrajectoryAdditional 
                        = additionalTrajectories.FirstOrDefault(x => trajectoriesLeft.Contains(x.X));
                    if ((maxTrajectoryOriginal?.Strain ?? 0d) >= (maxTrajectoryAdditional?.Strain ?? 0d))
                    {
                        if (maxTrajectoryOriginal is not (double X, double Strain) traj)
                        {
                            break;
                        }
                        strain += traj.Strain;
                        if (Double.IsNaN(strainOneVehicle))
                        {
                            strainOneVehicle = traj.Strain;
                        }
                        var removeTrajsFrom = traj.X - roadRule.MinTrajectoryDistance;
                        var removeTrajsTo = traj.X + roadRule.MinTrajectoryDistance;
                        trajectoriesLeft.RemoveWhere(t => removeTrajsFrom < t && t < removeTrajsTo);

                        if (!trajectoriesLeft.Contains(removeTrajsFrom) 
                            && intervalModel.PassageIntervalRef.AbsolutePositionLeft <= removeTrajsFrom
                            && removeTrajsFrom <= intervalModel.PassageIntervalRef.AbsolutePositionRight)
                        {
                            if (vehicleTrajectoryService.GetVehicleTrajectory(mesh, data.Load, removeTrajsFrom) is VehicleTrajectory additionalTrajectory)
                            {
                                additionalTrajectories.Add((removeTrajsFrom, trajectorySelector.GetStrainForEachPositivePiece(additionalTrajectory, data, roadRule.DoTrafficJamLoadCalulation).Max()));
                                trajectoriesLeft.Add(removeTrajsFrom);
                            }
                        }
                        if (!trajectoriesLeft.Contains(removeTrajsTo)
                            && intervalModel.PassageIntervalRef.AbsolutePositionLeft <= removeTrajsTo
                            && removeTrajsTo <= intervalModel.PassageIntervalRef.AbsolutePositionRight)
                        {
                            if (vehicleTrajectoryService.GetVehicleTrajectory(mesh, data.Load, removeTrajsTo) is VehicleTrajectory additionalTrajectory)
                            {
                                additionalTrajectories.Add((removeTrajsTo, trajectorySelector.GetStrainForEachPositivePiece(additionalTrajectory, data, roadRule.DoTrafficJamLoadCalulation).Max()));
                                trajectoriesLeft.Add(removeTrajsTo);
                            }
                        }
                    }
                    else
                    {
                        if (maxTrajectoryAdditional is not (double X, double Strain) traj)
                        {
                            break;
                        }
                        strain += traj.Strain;
                        if (Double.IsNaN(strainOneVehicle))
                        {
                            strainOneVehicle = traj.Strain;
                        }
                        var removeTrajsFrom = traj.X - roadRule.MinTrajectoryDistance / 2;
                        var removeTrajsTo = traj.X + roadRule.MinTrajectoryDistance / 2;
                        trajectoriesLeft.RemoveWhere(t => removeTrajsFrom < t && t < removeTrajsTo);

                        if (!trajectoriesLeft.Contains(removeTrajsFrom)
                            && intervalModel.PassageIntervalRef.AbsolutePositionLeft <= removeTrajsFrom
                            && removeTrajsFrom < intervalModel.PassageIntervalRef.AbsolutePositionRight)
                        {
                            if (vehicleTrajectoryService.GetVehicleTrajectory(mesh, data.Load, removeTrajsFrom) is VehicleTrajectory additionalTrajectory)
                            {
                                additionalTrajectories.Add((removeTrajsFrom, trajectorySelector.GetStrainForEachPositivePiece(additionalTrajectory, data, roadRule.DoTrafficJamLoadCalulation).Max()));
                                trajectoriesLeft.Add(removeTrajsFrom);
                            }
                        }
                        if (!trajectoriesLeft.Contains(removeTrajsTo)
                            && intervalModel.PassageIntervalRef.AbsolutePositionLeft <= removeTrajsTo
                            && removeTrajsTo < intervalModel.PassageIntervalRef.AbsolutePositionRight)
                        {
                            if (vehicleTrajectoryService.GetVehicleTrajectory(mesh, data.Load, removeTrajsTo) is VehicleTrajectory additionalTrajectory)
                            {
                                additionalTrajectories.Add((removeTrajsTo, trajectorySelector.GetStrainForEachPositivePiece(additionalTrajectory, data, roadRule.DoTrafficJamLoadCalulation).Max()));
                                trajectoriesLeft.Add(removeTrajsTo);
                            }
                        }
                    }
                } 
                yield return
                    new StrainResult
                    {
                        RoadRuleRef = roadRule,
                        Strain = strain,
                        StrainOneAuto = strainOneVehicle
                    };
            }
        }
    }
}
