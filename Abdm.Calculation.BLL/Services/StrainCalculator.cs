using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCalculator(IVehiclePositioner vehiclePositioner,
        IStrainCoefficientFactory strainCoefficientFactory,
        IEqualityComparer<double> equalityComparer,
        ITrajectoryFilterProvider trajectoryFilterProvider) : IStrainCalculator
    {
        public Dictionary<RoadRule, (double X, VehicleStrain strain)[]> GetStrainsMap(
            IntervalModel intervalModel,
            VehicleRollingBigModel bigData)
        {
            var data = bigData.Data;
            var roadRules = bigData.RoadRules;

            var strainMap = new Dictionary<double, VehicleStrain>(equalityComparer);
            var trafficJamStrainMap = new Dictionary<double, TrafficJamStrain?>(equalityComparer);
            var doTrafficJamStrainCalulation = roadRules.Any(r => r.DoTrafficJamLoadCalulation);

            foreach (var trajectory in intervalModel.Trajectories)
            {
                if (!strainMap.ContainsKey(trajectory.X))
                {
                    var vehicleStrains = GetStrainForEachPositivePiece(
                            trajectory,
                            data)
                        .Where(x => x != null)
                        .Cast<VehicleStrain>()
                        .ToArray();
                    if (vehicleStrains.Any())
                    {
                        strainMap[trajectory.X] = vehicleStrains.Max()!;
                    }
                }
                if (doTrafficJamStrainCalulation && !trafficJamStrainMap.ContainsKey(trajectory.X))
                {
                    trafficJamStrainMap[trajectory.X] = GetTrafficJamStrain(trajectory, data);
                }
            }

            var trajectoriesMap = new Dictionary<RoadRule, (double X, VehicleStrain strain)[]>();
            foreach (var roadRule in roadRules)
            {
                List<(double X, VehicleStrain strain)> strainList = new();
                var trajectoryFilter = trajectoryFilterProvider.GetFilter(intervalModel.PassageIntervalRef, data.Load, roadRule);
                foreach (var strains in strainMap.Where(s => trajectoryFilter.Filter(s.Key)))
                {
                    var vehicleStrain = strains.Value;
                    if (roadRule.DoTrafficJamLoadCalulation)
                    {
                        vehicleStrain.TrafficJamStrain = trafficJamStrainMap[strains.Key];
                    }
                    strainList.Add((strains.Key, strains.Value));
                }
                trajectoriesMap.Add(roadRule, strainList.OrderByDescending(s => s.strain.TotalStrain).ToArray());
            }
            
            return trajectoriesMap;
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
            var profileLeft = trajectory.Left.Last().Value;
            var profileRight = trajectory.Right.Last().Value;

            if (profileLeft.MaximumIndexes.Length == 0 && profileRight.MaximumIndexes.Length == 0)
            {
                yield return null;
            }

            ProfileYZ measuringProfile;
            if (profileLeft.PositivePieceMap.Values.Sum(interval => interval.End - interval.Start) > 
                profileRight.PositivePieceMap.Values.Sum(interval => interval.End - interval.Start))
            {
                measuringProfile = profileLeft;
            }
            else
            {
                measuringProfile = profileRight;
            }

            foreach (var maximumIndex in measuringProfile.MaximumIndexes)
            {
                var strain = vehiclePositioner.GetStrainFromVehicleInPosition(trajectory,
                    measuringProfile.Extremums[maximumIndex].X,
                    data);

                strain.LambdaSmall = strain.PositivePiecesMap[measuringProfile].Sum(interval => interval.End - interval.Start);

                if (strainCoefficientFactory.GetStrainCalculator(Enums.StrainCoefficientTypeEnum.BasicStrain, data.Surface.StrainCalculationGroupType) is ICoefficientCalculator coefficient)
                {
                    strain.Coefficient *= coefficient.Get(strain.LambdaSmall, data.Load.Type, data.Surface.Material);
                }

                yield return strain;
            }
        }

        public TrafficJamStrain GetTrafficJamStrain(VehicleTrajectory trajectory, VehicleRollingSmallModel data)
        {
            var curveLeft = trajectory.Left.Last().Value.GetYZ().ToArray();
            var curveRight = trajectory.Right.Last().Value.GetYZ().ToArray();
            var areaLeft = MathExtensions.CalculateAreaUnderCurve(curveLeft);
            var areaRight = MathExtensions.CalculateAreaUnderCurve(curveRight);

            var trafficJamStrain = new TrafficJamStrain();
            
            var totalWeight = data.Load.Axles.Sum(a => a.Weight);
            trafficJamStrain.LeftStrain = GetTraffciJamStrainForOneSide(areaLeft, totalWeight);
            trafficJamStrain.RightStrain = GetTraffciJamStrainForOneSide(areaRight, totalWeight);
            trafficJamStrain.SumStrain = trafficJamStrain.LeftStrain + trafficJamStrain.RightStrain;

            if (strainCoefficientFactory.GetStrainCalculator(Enums.StrainCoefficientTypeEnum.TrafficJam, data.Surface.StrainCalculationGroupType) is ICoefficientCalculator coefficient)
            {
                trafficJamStrain.Coefficient *= coefficient.Get(data.Surface.Lambda, data.Load.Type, data.Surface.Material);
            }

            return trafficJamStrain;
        }

        /// <summary>
        /// Расчёт равномерного напряжения для одной стороны ТС (левой или правой). 
        /// Делим на 2, так как вес сюда передается по сумме ВСЕХ, а нас интересуют лишь колёса слева (справа)
        /// </summary>
        private double GetTraffciJamStrainForOneSide(double area, double totalAxlesWeight)
        {
            return area * totalAxlesWeight * NormConstants.TrafficJamApproximationParam / 2;
        }
    }
}
