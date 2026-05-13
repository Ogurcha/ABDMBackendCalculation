using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCalculator(IVehiclePositioner vehiclePositioner,
        IStrainCoefficientFactory strainCoefficientFactory,
        IEqualityComparer<double> equalityComparer,
        ITrajectoryFilterProvider trajectoryFilterProvider) : IStrainCalculator
    {
        /// <summary>
        /// Рассчитывает карту напряжений на каждый <see cref="RoadRule"/> и на куждую <see cref="VehicleTrajectory"/>. 
        /// Дважды сортирует напряжения: 1) внутри траектории 2) и между траекториями
        /// </summary>
        public Dictionary<RoadRule, StrainsInTrajectory[]> GetStrainsMap(
            IntervalModel intervalModel,
            VehicleRollingBigModel bigData)
        {
            var data = bigData.Data;
            var roadRules = bigData.RoadRules;

            var strainMap = new Dictionary<double, (VehicleTrajectory traj, VehicleStrain[] strains)>(equalityComparer);
            var trafficJamStrainMap = new Dictionary<double, TrafficJamStrain?>(equalityComparer);
            var doTrafficJamStrainCalulation = roadRules.Any(r => r.DoTrafficJamLoadCalulation);

            foreach (var trajectory in intervalModel.Trajectories)
            {
                if (!strainMap.ContainsKey(trajectory.X) 
                    && TryGetStrainForEachPositivePiece(trajectory, data, out IEnumerable<VehicleStrain> vehicleStrains))
                {
                    strainMap[trajectory.X] = (trajectory, vehicleStrains.OrderDescending().ToArray()!);
                }
                if (doTrafficJamStrainCalulation && !trafficJamStrainMap.ContainsKey(trajectory.X))
                {
                    trafficJamStrainMap[trajectory.X] = GetTrafficJamStrain(trajectory, data);
                }
            }

            var trajectoriesMap = new Dictionary<RoadRule, StrainsInTrajectory[]>();
            foreach (var roadRule in roadRules)
            {
                List<StrainsInTrajectory> strainList = new();
                var trajectoryFilter = trajectoryFilterProvider.GetFilter(intervalModel.PassageIntervalRef, data.Load, roadRule);
                foreach (var strains in strainMap.Where(s => trajectoryFilter.Filter(s.Key)))
                {
                    var trafficJamStrain = roadRule.DoTrafficJamLoadCalulation
                        ? trafficJamStrainMap[strains.Key]
                        : null;
                    strainList.Add(new StrainsInTrajectory 
                    {
                        VehicleTrajectoryRef = strains.Value.traj, 
                        Strains = strains.Value.strains, 
                        TrafficJamStrain = trafficJamStrain,
                        TotalStrain = strains.Value.strains.First().TotalStrain + trafficJamStrain?.TotalStrain ?? 0d
                    });
                }
                trajectoriesMap.Add(roadRule, strainList.OrderDescending().ToArray());
            }
            
            return trajectoriesMap;
        }

        public bool TryGetStrainForEachPositivePiece(
            VehicleTrajectory trajectory,
            VehicleRollingSmallModel data,
            out IEnumerable<VehicleStrain> vehicleStrains)
        {
            vehicleStrains = GetStrainForEachPositivePiece(
                            trajectory,
                            data)
                        .Where(x => x != null)!;
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
                yield return null;
            }

            foreach (var maximumIndex in measuringProfile!.MaximumIndexes)
            {
                VehicleStrain strain = GetVehicleStrain(trajectory, data, measuringProfile, measuringProfile.Extremums[maximumIndex].X);

                yield return strain;
            }
        }

        public TrafficJamStrain GetTrafficJamStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data)
        {
            var areaLeft = trajectory.Left.Last().Value.PositivePieces.Sum(x => x.Length);
            var areaRight = trajectory.Right.Last().Value.PositivePieces.Sum(x => x.Length);

            var trafficJamStrain = new TrafficJamStrain();
            
            var totalWeight = data.Load.Axles.Sum(a => a.Weight);
            trafficJamStrain.LeftStrain = GetTraffciJamStrainForOneSide(areaLeft, totalWeight);
            trafficJamStrain.RightStrain = GetTraffciJamStrainForOneSide(areaRight, totalWeight);
            trafficJamStrain.SumStrain = trafficJamStrain.LeftStrain + trafficJamStrain.RightStrain;

            if (strainCoefficientFactory.GetStrainCalculator(Enums.StrainCoefficientTypeEnum.TrafficJam, data.Surface.StrainCalculationGroupType) is ICoefficientCalculator coefficient)
            {
                trafficJamStrain.Coefficient *= coefficient.Get(Math.Max(areaLeft, areaRight), data.Load.Type, data.Surface.Material);
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

            if (profileLeft.PositivePieceMap.Values.Sum(interval => interval.Length) > 
                profileRight.PositivePieceMap.Values.Sum(interval => interval.Length))
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

        private VehicleStrain GetVehicleStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data, ProfileYZ measuringProfile, double position)
        {
            var strain = vehiclePositioner.GetStrainFromVehicleInPosition(trajectory,
                                position,
                                data);

            strain.LambdaSmall = strain.PositivePiecesMap[measuringProfile].Sum(interval => interval.Length);

            if (strainCoefficientFactory.GetStrainCalculator(Enums.StrainCoefficientTypeEnum.BasicStrain, data.Surface.StrainCalculationGroupType) is ICoefficientCalculator coefficient)
            {
                strain.Coefficient *= coefficient.Get(strain.LambdaSmall, data.Load.Type, data.Surface.Material);
            }
            strain.TotalStrain = strain.SumStrain * strain.Coefficient;

            return strain;
        }
    }
}
