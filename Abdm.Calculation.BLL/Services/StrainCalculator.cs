using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Helpers;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainCalculator(IProfileYZService profileYZService,
        IVehiclePositioner vehiclePositioner) : IStrainCalculator
    {
        public Dictionary<RoadRule, (double X, double Strain)[]> GetStrainsMap(
            IntervalModel intervalModel,
            IEnumerable<RoadRule> roadRules,
            PassTypeSmallModel data)
        {
            var strainMap = new Dictionary<double, double>();

            var trajectoriesMap = new Dictionary<RoadRule, (double X, double strain)[]>();

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
                    if (!strainMap.ContainsKey(trajectory.X))
                    {
                        strainMap[trajectory.X] = GetStrainForEachPositivePiece(
                            trajectory, 
                            data, 
                            ruleGroup.Key.DoTrafficJamLoadCalulation).Max();
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
        public IEnumerable<double> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, PassTypeSmallModel data, bool doTrafficJamCalulation)
        {
            var centerVectors = profileYZService.GetYZFromProfile(trajectory.Center).ToArray();
            var positivePieces = MathExtensions.GetPositvePieces(centerVectors);

            if (positivePieces.Count() == 0)
            {
                yield return 0;
            }

            var trafficJamStrain = 0d;
            if (doTrafficJamCalulation)
            {
                var areaLeft = MathExtensions.CalculateAreaUnderCurve(profileYZService.GetYZFromProfile(trajectory.Left.Values.First()).ToArray());
                var areaRight = MathExtensions.CalculateAreaUnderCurve(profileYZService.GetYZFromProfile(trajectory.Right.Values.First()).ToArray());
                var areaAverage = (areaLeft + areaRight) / 2;

                trafficJamStrain += areaAverage
                    * data.Load.Axles.Sum(a => a.Weight)
                    * NormConstants.TrafficJamApproximationParam;
            }

            foreach (var positivePiece in positivePieces)
            {
                var start = positivePiece.X;
                var end = positivePiece.Y;

                var highestZVector = centerVectors.Where(v => v.X >= start && v.X <= end).OrderByDescending(v => v.Y).First();

                var strain = vehiclePositioner.GetStrainFromVehicleInPosition(trajectory,
                    highestZVector.X,
                    data.Load);
                yield return
                    strain * StrainCoefficientFormulas.GetBasicStrainCoefficient(data.Surface.Lambda)
                    + trafficJamStrain * StrainCoefficientFormulas.GetTrafficJamStrainCoefficient(data.Surface.Lambda);
            }
        }
    }
}
