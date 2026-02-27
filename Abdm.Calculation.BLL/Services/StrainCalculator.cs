using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Helpers;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCalculator(IProfileYZService profileYZService,
        IVehiclePositioner vehiclePositioner,
        IStrainCoefficientFactory strainCoefficientFactory) : IStrainCalculator
    {
        public Dictionary<RoadRule, (double X, VehicleStrain Strain)[]> GetStrainsMap(
            IntervalModel intervalModel,
            VehicleRollingBigModel bigData)
        {
            var data = bigData.Data;
            var roadRules = bigData.RoadRules;
            var strainMap = new Dictionary<double, VehicleStrain>();

            var trajectoriesMap = new Dictionary<RoadRule, (double X, VehicleStrain strain)[]>();

            var groupedBySafetyLine = roadRules.GroupBy(r => (
            actualSafetyLineLeft: r.HasSafetyLine ? intervalModel.PassageIntervalRef.SafetyLineLeft : (double)default,
            actualSafetyLineRight: r.HasSafetyLine ? intervalModel.PassageIntervalRef.SafetyLineRight : (double)default,
            r.DoTrafficJamLoadCalulation));

            foreach (var ruleGroup in groupedBySafetyLine)
            {
                var start = intervalModel.PassageIntervalRef.AbsolutePositionLeft
                + ruleGroup.Key.actualSafetyLineLeft
                + PassTypeFormulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(data.Load, ruleGroup);
                var finish = intervalModel.PassageIntervalRef.AbsolutePositionRight
                - ruleGroup.Key.actualSafetyLineRight
                - PassTypeFormulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(data.Load, ruleGroup);

                var actualTrajectories = intervalModel.Trajectories.Where(t => t.X >= start && t.X <= finish);

                foreach (var trajectory in actualTrajectories)
                {
                    if (!strainMap.ContainsKey(trajectory.X) 
                        && GetStrainForEachPositivePiece(
                            trajectory,
                            data,
                            ruleGroup.Key.DoTrafficJamLoadCalulation).Max() is VehicleStrain vehicleStrain)
                    {
                        strainMap[trajectory.X] = vehicleStrain;
                    }
                }

                foreach (var rule in ruleGroup)
                {
                    trajectoriesMap.Add(rule, actualTrajectories.OrderByDescending(t => strainMap[t.X])
                        .Select(t => (t.X, strainMap[t.X])).ToArray());
                }
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
        public IEnumerable<VehicleStrain?> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, VehicleRollingSmallModel data, bool doTrafficJamCalulation)
        {
            var centerVectors = profileYZService.GetYZFromProfile(trajectory.Center).ToArray();
            var positivePieces = MathExtensions.GetPositvePieces(centerVectors);

            if (positivePieces.Count() == 0)
            {
                yield return new VehicleStrain { SumStrain = 0, WheelStrains = [] };
            }

            TrafficJamStrain? trafficJamStrain = null;
            if (doTrafficJamCalulation)
            {
                var curveLeft = profileYZService.GetYZFromProfile(trajectory.Left.Values.First()).ToArray();
                var curveRight = profileYZService.GetYZFromProfile(trajectory.Right.Values.First()).ToArray();
                var areaLeft = MathExtensions.CalculateAreaUnderCurve(curveLeft);
                var areaRight = MathExtensions.CalculateAreaUnderCurve(curveRight);

                trafficJamStrain = new TrafficJamStrain 
                { 
                    LeftPieces = MathExtensions.GetPositvePieces(curveLeft).Select(x => 
                    new PositivePieceStrain { BeginY = x.X, EndY = x.Y }).ToArray(),
                    RightPieces = MathExtensions.GetPositvePieces(curveRight).Select(x => 
                    new PositivePieceStrain { BeginY = x.X, EndY = x.Y }).ToArray()
                };
                var totalWeight = data.Load.Axles.Sum(a => a.Weight);
                trafficJamStrain.LeftStrain = GetTraffciJamStrainForOneSide(areaLeft, totalWeight);
                trafficJamStrain.RightStrain = GetTraffciJamStrainForOneSide(areaRight, totalWeight);
                trafficJamStrain.SumStrain = trafficJamStrain.LeftStrain + trafficJamStrain.RightStrain;

                if (strainCoefficientFactory.GetStrainCalculator(Enums.StrainCoefficientTypeEnum.TrafficJam, data.Surface.StrainCalculationGroupType) is ICoefficientCalculator coefficient)
                {
                    trafficJamStrain.Coefficient *= coefficient.Get(data.Surface.Lambda, data.Load.Type, data.Surface.Material);
                }
            }

            foreach (var positivePiece in positivePieces)
            {
                var start = positivePiece.X;
                var end = positivePiece.Y;

                var highestZVector = centerVectors.Where(v => v.X >= start && v.X <= end).OrderByDescending(v => v.Y).First();

                var strain = vehiclePositioner.GetStrainFromVehicleInPosition(trajectory,
                    highestZVector.X,
                    data);
                strain.TrafficJamStrain = trafficJamStrain;

                if (strainCoefficientFactory.GetStrainCalculator(Enums.StrainCoefficientTypeEnum.BasicStrain, data.Surface.StrainCalculationGroupType) is ICoefficientCalculator coefficient)
                {
                    strain.Coefficient *= coefficient.Get(data.Surface.Lambda, data.Load.Type, data.Surface.Material);
                }

                yield return strain;
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
    }
}
