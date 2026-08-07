using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Services.LowLevelCalculation;
using Abdm.Calculation.Maths.Helpers;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCalculator(
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
                    var totalStrain = strains.Value.strains.First().TotalStrain + (trafficJamStrain?.TotalStrain ?? 0d);
                    if (totalStrain > NormConstants.MinimalTrajectoryStrain)
                    {
                        strainList.Add(new StrainsInMaximums
                        {
                            VehicleTrajectoryRef = strains.Value.traj,
                            Strains = strains.Value.strains,
                            TrafficJamStrain = trafficJamStrain,
                            TotalStrain = totalStrain
                        });
                    }
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
            var measuringProfile = PassTypeFormulas.GetMeasuringProfile(trajectory);
            if (measuringProfile == null)
            {
                yield break;
            }

            foreach (var maximumIndex in measuringProfile!.MaximumIndexes)
            {
                VehicleStrain strain = GetVehicleStrain(trajectory, data, measuringProfile, measuringProfile.Extremums[maximumIndex].X);

                if (strain.TotalStrain > NormConstants.MinimalTrajectoryStrain)
                {
                    yield return strain;
                }
            }
            yield return GetVehicleStrain(trajectory, data, measuringProfile, NormConstants.YYY);
        }

        private TrafficJamStrain GetTrafficJamStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data)
        {
            if (trajectory.X == NormConstants.XXX)
            {

            }

            var trafficJamStrain = new TrafficJamStrain()
            {
                SumStrain = 0d,
                TotalStrain = 0d,
                StrainPieces = []
            }; 
            var trafficJamStrainProvider = data.VehicleStrainProvider as VehicleStrainProviderVolumetric;
            var measuringPorifle = PassTypeFormulas.GetMeasuringProfile(trajectory);
            if (measuringPorifle == null || trafficJamStrainProvider == null)
            {
                return trafficJamStrain;
            }

            foreach (var positivePiece in measuringPorifle.PositivePieces)
            {
                trafficJamStrain.StrainPieces.Add(GetTrafficJamStrainPiece(trajectory, data, positivePiece));
            }
            trafficJamStrain.SumStrain = trafficJamStrain.StrainPieces.Sum(p => p.LeftStrain + p.RightStrain);

            if (data.CoefficientProvider.TrafficJamStrainCoefficientProvider != null)
            {
                trafficJamStrain.ReliabilityCoefficient = data.CoefficientProvider.TrafficJamStrainCoefficientProvider.GetBasicCoefficent(trajectory.Center.PositivePieces.Sum(x => x.Length));
            }
            trafficJamStrain.TotalStrain = trafficJamStrain.SumStrain * trafficJamStrain.ReliabilityCoefficient;

            return trafficJamStrain;

            TrafficJamStrainPiece GetTrafficJamStrainPiece(VehicleTrajectory trajectory, VehicleRollingSmallModel data, Interval interval)
            {
                var leftVolume = 0d;
                var rightVolume = 0d;
                var leftStrain = 0d;
                var rightStrain = 0d;
                

                foreach (var wheelOffset in data.Load.WheelOffsetsMap!.Keys)
                {
                    (double volume, double strain) = 
                        trafficJamStrainProvider.GetTrafficJamStrainForOneProfile(
                            (ProfileYZExtended)trajectory.Left[wheelOffset * 2], 
                            data.Load, 
                            interval,
                            wheelOffset);
                    leftVolume += volume;
                    leftStrain += strain;
                    (volume, strain) = 
                        trafficJamStrainProvider.GetTrafficJamStrainForOneProfile(
                            (ProfileYZExtended)trajectory.Right[wheelOffset * 2], 
                            data.Load,
                            interval,
                            wheelOffset);
                    rightVolume += volume;
                    rightStrain += strain;
                }

                return new TrafficJamStrainPiece
                {
                    Interval = interval,
                    LeftStrain = leftStrain,
                    RightStrain = rightStrain,
                    LeftVolume = leftVolume,
                    RightVolume = rightVolume,
                };
            }
        }

        ///// <summary>
        ///// Расчёт равномерного напряжения для одной стороны ТС (левой или правой) для одной оси/тележки
        ///// </summary>
        //private double GetTraffciJamVolumeForOneSide(ProfileYZExtended profile, Axle axle)
        //{
        //    double totalVolume = 0d;
        //    double? previousArea = null; double? previousPosition = null;
        //    double currentArea; double currentPosition;
        //    for (int i = 0; i < profile.VolumetricProfiles[axle].Length; i++)
        //    {
        //        currentArea = MathExtensions.CalculateAreaUnderCurve(profile.VolumetricProfiles[axle][i].SortedVectors);
        //        currentPosition = profile.VolumetricProfiles[axle][i].X;
        //        if (previousArea != null)
        //        {
        //            totalVolume += MathExtensions.FrustrumVolume(currentPosition - previousPosition!.Value, previousArea.Value, currentArea);
        //        }
        //        previousArea = currentArea;
        //        previousPosition = currentPosition;
        //    }

        //    return totalVolume;
        //}

        private VehicleStrain GetVehicleStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data, ProfileYZ measuringProfile, double position)
        {
            var strain = PassTypeFormulas.GetStrainFromVehicleInPosition(trajectory,
                                position,
                                data);

            strain.LambdaSmall = strain.PositivePiecesMap[measuringProfile].Sum(interval => interval.Length);

            strain.ReliabilityCoefficient = data.CoefficientProvider.GetBasicCoefficent(strain.LambdaSmall);
            strain.TotalStrain = strain.SumStrain * strain.ReliabilityCoefficient;

            return strain;
        }
    }
}
