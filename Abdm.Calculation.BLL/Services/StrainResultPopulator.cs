using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainResultPopulator(IVehiclePositioner vehiclePositioner,
        IEqualityComparer<double> equalityComparer) : IStrainResultPopulator
    {
        public StrainResult PopulateStrainResult(StrainResultUnpopulated unpopulated, VehicleRollingSmallModel data)
        {
            return new StrainResult
            {
                RoadRuleRef = unpopulated.RoadRuleRef,
                Strain = new VehicleColumnStrainList(unpopulated.Strain.Select(PopulateIndividualColumn)),
                StrainOneAuto = PopulateIndividualColumn(unpopulated.StrainOneAuto)
            };

            VehicleColumnStrain PopulateIndividualColumn(StrainsInTrajectory traj)
            {
                var vehicleStrainList = new List<VehicleStrain>();
                double effectiveLoadDistance = data.Load.Length + data.Load.Distance;
                foreach (var vehicleStrain in traj.Strains)
                {
                    vehicleStrainList.Add(vehicleStrain);
                    var distanceFromExtremum = effectiveLoadDistance;
                    bool isValidMax = vehicleStrain.Position + distanceFromExtremum
                        <= data.Surface.MaxY + data.Load.Length;
                    bool isValidMin = vehicleStrain.Position - distanceFromExtremum
                        >= data.Surface.MinY - data.Load.Length;
                    while (isValidMax || isValidMin)
                    {
                        if (isValidMax)
                        {
                            CloneVehicleStrain(vehicleStrain, distanceFromExtremum);
                        }
                        if (isValidMin)
                        {
                            CloneVehicleStrain(vehicleStrain, -distanceFromExtremum);
                        }
                        distanceFromExtremum += effectiveLoadDistance;
                    }
                }
                vehicleStrainList = vehicleStrainList.OrderDescending().ToList();
                var vehicleStrainPositions = vehicleStrainList.Select(x => x.Position).ToHashSet(equalityComparer);

                var resultStrains = new List<VehicleStrain>();

                var maxVehiclesInColumn = unpopulated.RoadRuleRef.MaxVehicleInTrajectory;
                for (int i = 0; i < vehicleStrainList.Count; i++)
                {
                    if (!vehicleStrainPositions.Any() || resultStrains.Count >= maxVehiclesInColumn)
                    {
                        break;
                    }
                    var strainToAdd = vehicleStrainList[i];
                    if (vehicleStrainPositions.Contains(strainToAdd.Position))
                    {
                        resultStrains.Add(strainToAdd);
                        foreach (var position in vehicleStrainPositions.ToArray())
                        {
                            if (position <= strainToAdd.Position + effectiveLoadDistance && position >= strainToAdd.Position - effectiveLoadDistance)
                            {
                                vehicleStrainPositions.Remove(position);
                            }
                        }
                    }
                }

                return new VehicleColumnStrain
                {
                    VehicleTrajectoryRef = traj.VehicleTrajectoryRef,
                    TrafficJamStrain = traj.TrafficJamStrain,
                    VehicleStrains = resultStrains.ToArray(),
                    TotalStrain = resultStrains.Sum(x => x.TotalStrain) + (traj.TrafficJamStrain?.TotalStrain ?? 0d)
                };

                void CloneVehicleStrain(VehicleStrain vehicleStrain, double distanceFromExtremum)
                {
                    var strain = vehiclePositioner.GetStrainFromVehicleInPosition(traj.VehicleTrajectoryRef, vehicleStrain.Position + distanceFromExtremum, data);
                    if (strain != null && strain.SumStrain > 0d)
                    {     
                        strain.Coefficient = vehicleStrain.Coefficient;
                        strain.LambdaSmall = vehicleStrain.LambdaSmall;
                        strain.TotalStrain = strain.SumStrain * strain.Coefficient;
                        vehicleStrainList.Add(strain);
                    }
                }
            }
        }
    }
}
