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
        /// Пользуясь фактом того, что пики поверхности влияния чередуются с отрицательными зонами, 
        /// мы можем найти все потенциальные пики вырезая положильные куски графика. 
        /// Данный метод делит траекторию на положительные отрезки, 
        /// чтобы проверить все пики и выдать напряжение по каждому из них
        /// </summary>
        public IEnumerable<VehicleStrain?> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, 
            VehicleRollingSmallModel data)
        {
            var curveLeft = trajectory.Left.Last().Value.GetYZ().ToArray();
            var curveRight = trajectory.Right.Last().Value.GetYZ().ToArray();
            var positivePiecesLeft = MathExtensions.GetPositvePieces(curveLeft)
                .Where(p => data.Load.Length < p.Y - p.X)
                .ToArray();
            var positivePiecesRight = MathExtensions.GetPositvePieces(curveRight)
                .Where(p => data.Load.Length < p.Y - p.X)
                .ToArray();

            ///короче вместо положительных кусков, где просто использовать поло

            if (positivePiecesLeft.Length == 0 && positivePiecesRight.Length == 0)
            {
                yield return null;
            }

            Vector2D[] positivePieces;
            Vector2D[] curve;
            if (positivePiecesLeft.Sum(v => v.Y - v.X) > positivePiecesRight.Sum(v => v.Y - v.X))
            {
                positivePieces = positivePiecesLeft;
                curve = curveLeft;
            }
            else
            {
                positivePieces = positivePiecesRight;
                curve = curveRight;
            }

            foreach (var positivePiece in positivePieces)
            {
                var start = positivePiece.X;
                var end = positivePiece.Y;

                var highestZVector = curve.Where(v => v.X >= start && v.X <= end).OrderByDescending(v => v.Y).First();

                var strain = vehiclePositioner.GetStrainFromVehicleInPosition(trajectory,
                    highestZVector.X,
                    data);

                if (strainCoefficientFactory.GetStrainCalculator(Enums.StrainCoefficientTypeEnum.BasicStrain, data.Surface.StrainCalculationGroupType) is ICoefficientCalculator coefficient)
                {
                    strain.Coefficient *= coefficient.Get(data.Surface.Lambda, data.Load.Type, data.Surface.Material);
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
