using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Extensions;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCalculator(IVehiclePositioner vehiclePositioner,
        IEqualityComparer<double> equalityComparer,
        ITrajectoryFilterProvider trajectoryFilterProvider) : IStrainCalculator
    {
        /// <summary>
        /// Рассчитывает карту напряжений на каждый <see cref="RoadRule"/> и на куждую <see cref="VehicleTrajectory"/>. 
        /// Cортирует напряжения внутри траектории по убыванию
        /// </summary>
        public List<StrainMap> GenerateStrainsMap(
            IntervalModel intervalModel,
            VehicleRollingBigModel bigData)
        {
            var data = bigData.Data;
            var roadRules = bigData.RoadRules;

            var strainMap = new Dictionary<double, (VehicleTrajectory traj, VehicleStrain[] strains)>(equalityComparer);
            var trafficJamStrainMap = new Dictionary<double, TrafficJamStrain?>(equalityComparer);
            var doTrafficJamStrainCalculation = roadRules.Any(r => r.DoTrafficJamLoadCalculation);

            foreach (var trajectory in intervalModel.Trajectories)
            {
                if (!strainMap.ContainsKey(trajectory.X) 
                    && TryGetStrainForEachPositivePiece(trajectory, data, out VehicleStrain[] vehicleStrains))
                {
                    strainMap[trajectory.X] = (trajectory, vehicleStrains);
                }
                if (doTrafficJamStrainCalculation && !trafficJamStrainMap.ContainsKey(trajectory.X))
                {
                    trafficJamStrainMap[trajectory.X] = GetTrafficJamStrain(trajectory, data);
                }
            }

            var result = new List<StrainMap>();
            foreach (var roadRule in roadRules)
            {
                List<StrainsInMaximums> strainList = new();
                var trajectoryFilter = trajectoryFilterProvider.GetFilter(intervalModel.PassageIntervalRef, data.Load, roadRule);
                foreach (var strains in strainMap.Where(s => trajectoryFilter.Filter(s.Key)))
                {
                    var trafficJamStrain = roadRule.DoTrafficJamLoadCalculation
                        ? trafficJamStrainMap[strains.Key]
                        : null;
                    strainList.Add(new StrainsInMaximums 
                    {
                        VehicleTrajectoryRef = strains.Value.traj, 
                        Strains = strains.Value.strains, 
                        TrafficJamStrain = trafficJamStrain,
                        TotalStrain = strains.Value.strains.First().TotalStrain + (trafficJamStrain?.TotalStrain ?? 0d)
                    });
                }
                result.Add(new StrainMap() {
                    RoadRuleRef = roadRule,
                    IntervalModelRef = intervalModel,
                    StrainsInMaximums = strainList.ToArray()
                });
            }
            
            return result;
        }

        public bool TryGetStrainForEachPositivePiece(
            VehicleTrajectory trajectory,
            VehicleRollingSmallModel data,
            out VehicleStrain[] vehicleStrains)
        {
            vehicleStrains = GetStrainForEachPositivePiece(
                            trajectory,
                            data)
                        .Where(x => x != null)
                        .OrderDescending()
                        .ToArray()!;
            if (vehicleStrains.Any()) 
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ИССО может быть устроена таким образом, 
        /// что более высокий пик в поверхности влияния выдаст меньшее напряжение из-за того, 
        /// что края высокого пика могут опускаться в ноль слишком резко, 
        /// в то время, как более низкий, 
        /// но более пологий пик выдаст напряжение больше. 
        /// В связи с этим напряжение ищется во всех локальных максимумах профиля.
        /// Профиль выбирается тот, с какой стороны больше площадь положи
        /// </summary>
        public IEnumerable<VehicleStrain?> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, 
            VehicleRollingSmallModel data)
        {
            var measuringProfile = GetMeasuringProfile(trajectory);
            if (measuringProfile == null)
            {
                yield break;
            }

            foreach (var maximumIndex in measuringProfile!.MaximumIndexes)
            {
                VehicleStrain strain = GetVehicleStrain(trajectory, data, measuringProfile, measuringProfile.Extremums[maximumIndex].X);

                yield return strain;
            }
        }

        public TrafficJamStrain GetTrafficJamStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data)
        {
            var trafficJamStrain = new TrafficJamStrain()
            {
                LeftStrain = 0d,
                RightStrain = 0d,
                SumStrain = 0d,
                TotalStrain = 0d,
            };
            if (trajectory.Left.First().Value is not ProfileYZExtended profileLeft || trajectory.Right.First().Value is not ProfileYZExtended profileRight)
            {
                return trafficJamStrain;
            }

            foreach (var wheelOffset in data.Load.WheelOffsetsMap!)
            {
                var axle = data.Load.Axles.Where(a => a.WheelsDistance.Contains(wheelOffset.Key * 2)).OrderByDescending(x => x.WheelWidth).First();
                var profileWeight = wheelOffset.Value.Item2 * NormConstants.TrafficJamApproximationParam;

                var volumeLeft = GetTraffciJamVolumeForOneSide(profileLeft, axle);
                var volumeRight = GetTraffciJamVolumeForOneSide(profileRight, axle);

                
                trafficJamStrain.LeftVolume += volumeLeft;
                trafficJamStrain.LeftStrain += volumeLeft * profileWeight / profileLeft.FootprintWidth[axle];
                trafficJamStrain.RightVolume += volumeRight;
                trafficJamStrain.RightStrain += volumeRight * profileWeight / profileRight.FootprintWidth[axle];
                trafficJamStrain.SumStrain += trafficJamStrain.LeftStrain + trafficJamStrain.RightStrain;
            }

            if (data.CoefficientProvider.TrafficJamStrainCoefficientProvider != null)
            {
                trafficJamStrain.ReliabilityCoefficient = data.CoefficientProvider.TrafficJamStrainCoefficientProvider.GetBasicCoefficent(trajectory.Center.PositivePieces.Sum(x => x.Length));
            }
            trafficJamStrain.TotalStrain = trafficJamStrain.SumStrain * trafficJamStrain.ReliabilityCoefficient;

            return trafficJamStrain;
        }

        private ProfileYZ? GetMeasuringProfile(VehicleTrajectory trajectory)
        {
            var profileLeft = trajectory.Left.Last().Value;
            var profileRight = trajectory.Right.Last().Value;

            if (profileLeft.MaximumIndexes.Length == 0 && profileRight.MaximumIndexes.Length == 0)
            {
                return null;
            }

            if (profileLeft.PositivePieces.Sum(interval => interval.Length) 
                * profileLeft.Extremums.DefaultIfEmpty().Max(v => v.Y) 
                > 
                profileRight.PositivePieces.Sum(interval => interval.Length) 
                * profileRight.Extremums.DefaultIfEmpty().Max(v => v.Y))
            {
                return profileLeft;
            }
            else
            {
                return profileRight;
            }
        }

        /// <summary>
        /// Расчёт равномерного напряжения для одной стороны ТС (левой или правой) для одной оси/тележки
        /// </summary>
        private double GetTraffciJamVolumeForOneSide(ProfileYZExtended profile, Axle axle)
        {
            double totalVolume = 0d;
            double? previousArea = null; double? previousPosition = null;
            double currentArea; double currentPosition;
            for (int i = 0; i < profile.VolumetricProfiles[axle].Length; i++)
            {
                currentArea = MathExtensions.CalculateAreaUnderCurve(profile.VolumetricProfiles[axle][i].SortedVectors);
                currentPosition = profile.VolumetricProfiles[axle][i].X;
                if (previousArea != null)
                {
                    totalVolume += MathExtensions.FrustrumVolume(currentPosition - previousPosition!.Value, previousArea.Value, currentArea);
                }
                previousArea = currentArea;
                previousPosition = currentPosition;
            }

            return totalVolume;
        }

        private VehicleStrain GetVehicleStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data, ProfileYZ measuringProfile, double position)
        {
            var strain = vehiclePositioner.GetStrainFromVehicleInPosition(trajectory,
                                position,
                                data);

            strain.LambdaSmall = strain.PositivePiecesMap[measuringProfile].Sum(interval => interval.Length);

            strain.ReliabilityCoefficient = data.CoefficientProvider.GetBasicCoefficent(strain.LambdaSmall);
            strain.TotalStrain = strain.SumStrain * strain.ReliabilityCoefficient;
            if (strain.InvertedDirectionStrain != null)
            {
                strain.InvertedDirectionStrain.LambdaSmall = strain.LambdaSmall;
                strain.InvertedDirectionStrain.ReliabilityCoefficient = strain.ReliabilityCoefficient;
                strain.InvertedDirectionStrain.TotalStrain = strain.InvertedDirectionStrain.SumStrain * strain.InvertedDirectionStrain.ReliabilityCoefficient;
            }

            return strain;
        }
    }
}
