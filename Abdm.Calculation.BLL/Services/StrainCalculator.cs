using System.Data;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Сервис для рассчетов напряжения в колонне транспорта
    /// </summary>
    public class StrainCalculator(IProfileYZService profileYZService,
        IVehiclePositioner vehiclePositioner) : IStrainCalculator
    {
        public static double TrafficJamCoefficient = 1.35d;

        public List<(IPassTypeCondition condition, PassTypeEnum passType)> PassTypeConditions =
            new()
            {
                (new NoLimitCondition(), PassTypeEnum.NoLimit),
                (new WithoutPedestrianCondition(), PassTypeEnum.WithoutPedestian),
                (new Speed10Condition(), PassTypeEnum.MaxSpeed10),
                (new SingleAutoOnlyCondition(), PassTypeEnum.SingleAutoOnly)
            };

        /// TODO: пока неизвестен алгоритм реализации расчётов по нормам при значении <see cref="RoadRule.MaxVehicleInTrajectory"/> больше 1
        /// Поэтому будем пока считать, что в колонне ТС всегда 1

        public PassTypeEnum GetPassType(PassTypeSmallModel data, List<IntervalModel> intervalModels, RoadRule[] roadRules)
        {
            var resultStrains = new List<StrainResult>();

            foreach (var intervalModel in intervalModels)
            {
                var strainMap = new Dictionary<double, double>();
                var trajectoriesMap = new Dictionary<RoadRule, (double X, double strain)[]>();

                var groupedBySafetyLine = roadRules.GroupBy(r => (
                actualSafetyLineLeft: r.HasSafetyLine ? intervalModel.PassageIntervalRef.SafetyLineLeft : 0d,
                actualSafetyLineRight: r.HasSafetyLine ? intervalModel.PassageIntervalRef.SafetyLineRight : 0d));
                foreach (var ruleGroup in groupedBySafetyLine)
                {
                    var actualTrajectories = intervalModel.Trajectories.Where(t =>
                    t.X >= intervalModel.PassageIntervalRef.AbsolutePositionLeft 
                    + ruleGroup.Key.actualSafetyLineLeft 
                    + Formulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(data.Load, ruleGroup)
                    && t.X <= intervalModel.PassageIntervalRef.AbsolutePositionRight 
                    - ruleGroup.Key.actualSafetyLineRight 
                    - Formulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(data.Load, ruleGroup));

                    foreach (var trajectory in actualTrajectories)
                    {
                        if (!strainMap.ContainsKey(trajectory.X))
                        {
                            strainMap[trajectory.X] = GetStrainForEachPositivePiece(trajectory, data.Load).Max();
                        }
                    }

                    foreach (var rule in ruleGroup)
                    {
                        trajectoriesMap.Add(rule, actualTrajectories.OrderByDescending(t => strainMap[t.X])
                            .Select(t => (t.X, strainMap[t.X])).ToArray());
                    }   
                }

                
                foreach (var roadRule in roadRules)
                {
                    var actualVehicleCount = Math.Min(roadRule.MaxVehicleCount, intervalModel.PassageIntervalRef.LaneCount);
                    resultStrains.Add(
                        new StrainResult
                        {
                            RoadRuleRef = roadRule,
                            Strain = trajectoriesMap[roadRule].Take(actualVehicleCount)
                                .Sum(x => x.strain * (roadRule.DoTrafficJamLoadCalulation ? TrafficJamCoefficient : 1)),
                            StrainOneAuto = trajectoriesMap[roadRule].First().strain * (roadRule.DoTrafficJamLoadCalulation ? TrafficJamCoefficient : 1)
                        });
                }
            }

            return GetPassType(resultStrains, data.Surface);
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
        private IEnumerable<double> GetStrainForEachPositivePiece(VehicleTrajectory trajectory, LoadModel load)
        {
            var centerVectors = profileYZService.GetYZFromProfile(trajectory.Center).ToArray();
            var positivePieces = MathExtensions.GetPositvePieces(centerVectors);

            foreach (var positivePiece in positivePieces)
            {
                var start = positivePiece.X;
                var end = positivePiece.Y;

                var highestZVector = centerVectors.Where(v => v.X <= start && v.X >= end).OrderBy(v => v.Y).First();

                yield return vehiclePositioner.GetStrainFromVehicleInPosition(trajectory,
                    highestZVector.X,
                    load);
            }
        }
       

        private PassTypeEnum GetPassType(List<StrainResult> strainResults, SurfaceModel surfaceModel)
        {
            foreach (var c in PassTypeConditions)
            {
                if (c.condition.CanPassCondition(strainResults, surfaceModel))
                {
                    return c.passType;
                }
            }

            return PassTypeEnum.Denied;
        }

    }
}
