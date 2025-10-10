using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.GraphicsServices;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Сервис для рассчетов напряжения в колонне
    /// </summary>
    /// <param name="profileYZService"></param>
    public class StrainCalculator(IProfileYZService profileYZService,
        IVehiclePositioner vehiclePositioner) : IStrainCalculator
    {
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
            foreach (var intervalModel in intervalModels)
            {
                var actualTrajectories = intervalModel.Trajectories;
                if (roadRules.All(x => x.HasSafetyLine))
                {
                    actualTrajectories = actualTrajectories.Where(t =>
                    t.X >= intervalModel.PassageIntervalRef.AbsolutePositionLeft + intervalModel.PassageIntervalRef.SafetyLineLeft + Formulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(data.Load, roadRules)
                    && t.X <= intervalModel.PassageIntervalRef.AbsolutePositionRight - intervalModel.PassageIntervalRef.SafetyLineRight - Formulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(data.Load, roadRules))
                        .ToArray();
                }

                var strains = new List<(double strain, VehicleTrajectory trajectory)>();
                foreach (var trajectory in intervalModel.Trajectories)
                {
                    var centerVectors = profileYZService.GetYZFromProfile(trajectory.Center).ToArray();
                    var positivePieces = MathExtensions.GetPositvePieces(centerVectors);

                    foreach (var positivePiece in positivePieces)
                    {
                        var start = positivePiece.X;
                        var end = positivePiece.Y;

                        var highestZVector = centerVectors.Where(v => v.X <= start && v.X >= end).OrderBy(v => v.Y).First() ;

                        var strainInPositivePiece = vehiclePositioner.GetStrainFromVehicleInPosition(trajectory,
                            highestZVector.X,
                            data.Load);
                    }

                    strains.Add((strain, trajectory));
                }

                strains = strains.OrderBy(s => s.strain).ToList();
                var maxStrainResults = new List<(double strain, RoadRule roadRule)>();

                
                foreach (var roadRule in roadRules)
                {
                    var strain = 0d;
                    var actualVehicleCount = Math.Min(roadRule.MaxVehicleCount, intervalModel.PassageIntervalRef.LaneCount);
                    
                    if

                }
            }
        }




        

        private PassTypeEnum GetPassType(IEnumerable<StrainResult> strainResultData, Surface surfaceData, RoadRule[] roadRules)
        {
            foreach (var roadRule in roadRules)
            {
                var strainResults = strainResultData
                    .Where(x => x.RoadRuleRef == roadRule)
                    .OrderByDescending(c => c.Strain)
                    .ToList();
                foreach (var c in PassTypeConditions)
                {
                    if (c.condition.CanPassCondition(strainResults, surfaceData))
                    {
                        return c.passType;
                    }
                }
            }

            return PassTypeEnum.Denied;
        }

    }
}
