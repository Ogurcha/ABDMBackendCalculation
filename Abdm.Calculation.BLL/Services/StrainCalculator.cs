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
        public Dictionary<RoadRule, StrainsInMaximums[]> GenerateStrainsMap(
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

            var trajectoriesMap = new Dictionary<RoadRule, StrainsInMaximums[]>();
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
                trajectoriesMap.Add(roadRule, strainList.ToArray());
            }
            
            return trajectoriesMap;
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
            //TODO#2: Доделать ProfileYZExtended для случая, если в нагрузке много и РАЗНЫХ Axle
            //Пока что берём только первый слой
            if (trajectory.Left.First().Value is not ProfileYZExtended profileLeft || trajectory.Right.First().Value is not ProfileYZExtended profileRight)
            {
                return new TrafficJamStrain
                {
                    LeftStrain = 0d,
                    RightStrain = 0d,
                    SumStrain = 0d,
                    TotalStrain = 0d,
                };
            }
            //TODO#2:
            var wheelOffset = data.Load.WheelOffsetsMap!.First();

            var distanceFromCenter = wheelOffset.Key;
            var (wheelCount, profileWeight) = wheelOffset.Value;

            var volumeLeft = GetTraffciJamVolumeForOneSide(profileLeft);
            var volumeRight = GetTraffciJamVolumeForOneSide(profileRight);

            var trafficJamStrain = new TrafficJamStrain();
            
            var profileCoefficient = profileWeight * NormConstants.TrafficJamApproximationParam;
            trafficJamStrain.LeftVolume = volumeLeft;
            trafficJamStrain.LeftStrain = volumeLeft * profileCoefficient / profileLeft.FootprintWidth;
            trafficJamStrain.RightVolume = volumeRight;
            trafficJamStrain.RightStrain = volumeRight * profileCoefficient / profileRight.FootprintWidth;
            trafficJamStrain.SumStrain = trafficJamStrain.LeftStrain + trafficJamStrain.RightStrain;

            if (data.CoefficientProvider.TrafficJamStrainCoefficientProvider != null)
            {
                trafficJamStrain.Coefficient = data.CoefficientProvider.TrafficJamStrainCoefficientProvider.GetBasicCoefficent(trajectory.Center.PositivePieces.Sum(x => x.Length));
            }
            trafficJamStrain.TotalStrain = trafficJamStrain.SumStrain * trafficJamStrain.Coefficient;

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
        /// Расчёт равномерного напряжения для одной стороны ТС (левой или правой). 
        /// Делим на 2, так как вес сюда передается по сумме ВСЕХ, а нас интересуют лишь колёса слева (справа)
        /// </summary>
        private double GetTraffciJamStrainForOneSide(double area, double totalAxlesWeight)
        {
            return area * totalAxlesWeight * NormConstants.TrafficJamApproximationParam / 2;
        }

        private double GetTraffciJamVolumeForOneSide(ProfileYZExtended profile)
        {
            var trapezoidAreaLeft = MathExtensions.CalculateAreaUnderCurve(profile.SortedVectorsLeft);
            var trapezoidAreaRight = MathExtensions.CalculateAreaUnderCurve(profile.SortedVectorsRight);
            var trapezoidAreaCenter = MathExtensions.CalculateAreaUnderCurve(profile.SortedVectors);

            var volume1 = MathExtensions.FrustrumVolume(profile.FootprintWidth / 2, trapezoidAreaLeft, trapezoidAreaCenter);
            var volume2 = MathExtensions.FrustrumVolume(profile.FootprintWidth / 2, trapezoidAreaRight, trapezoidAreaCenter);

            return volume1 + volume2;
        }

        private VehicleStrain GetVehicleStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data, ProfileYZ measuringProfile, double position)
        {
            var strain = vehiclePositioner.GetStrainFromVehicleInPosition(trajectory,
                                position,
                                data);

            strain.LambdaSmall = strain.PositivePiecesMap[measuringProfile].Sum(interval => interval.Length);

            strain.Coefficient = data.CoefficientProvider.GetBasicCoefficent(strain.LambdaSmall);
            strain.TotalStrain = strain.SumStrain * strain.Coefficient;
            if (strain.InvertedDirectionStrain != null)
            {
                strain.InvertedDirectionStrain.LambdaSmall = strain.LambdaSmall;
                strain.InvertedDirectionStrain.Coefficient = strain.Coefficient;
                strain.InvertedDirectionStrain.TotalStrain = strain.InvertedDirectionStrain.SumStrain * strain.InvertedDirectionStrain.Coefficient;
            }

            return strain;
        }
    }
}
