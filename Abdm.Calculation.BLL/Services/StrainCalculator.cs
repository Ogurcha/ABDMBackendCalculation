using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Extensions;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Primitives;
using Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions;

namespace Abdm.Calculation.BLL.Services
{
    /// <summary>
    /// Сервис для рассчетов напряжения в колонне
    /// </summary>
    /// <param name="profileYZService"></param>
    public class StrainCalculator(IProfileYZService profileYZService) : IStrainCalculator
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

                foreach (var trajectory in intervalModel.Trajectories)
                {
                    var centerVectors = profileYZService.GetYZFromProfile(trajectory.Center).ToArray();
                    var positiveIntervals = MathExtensions.GetPositveIntervals(centerVectors);

                    foreach (var positiveInterval in positiveIntervals)
                    {
                        var start = positiveInterval.X;
                        var end = positiveInterval.Y;

                        var highestZVector = centerVectors.Where(v => v.X <= start && v.X >= end).OrderBy(v => v.Y).First();

                        var strain = 
                    }
                }
            }
        }




        public StrainResult CalculateColumnModel(
            [DisallowNull] VehicleTrajectory[] vehicleTrajectories,
            LoadSchema loadSchema, 
            RoadRules roadRules)
        {
            var column = new StrainDataContainer(vehicleTrajectories);

            if (vehicleTrajectories.Length == 0)
            {
                return column;
            }

            //foreach (var trajectory in vehicleTrajectories) 
            //{
            //    CalculateStrain(column, trajectory);
            //}

            return column;
        }
        /*VehicleDistance = Math.Max(inputData.LoadSchema.Distance ?? DefaultVehicleDistance, roadRule.MinColumnDistance),
                    DistanceForSafetyLineLeft = roadRule.HasSafetyLine ? i.SafetyLineLeft : 0,
                    DistanceForSafetyLineRight = roadRule.HasSafetyLine ? i.SafetyLineRight : 0,
                    AbsolutePositionLeft = roadRule.HasSafetyLine ? i.AbsolutePositionLeft + i.SafetyLineLeft : i.AbsolutePositionLeft,
                    AbsolutePositionRight = roadRule.HasSafetyLine ? i.AbsolutePositionRight - i.SafetyLineLeft : i.AbsolutePositionLeft,
                    LaneCount = Math.Min(i.LaneCount, roadRule.MaxColumnCount)
        */


        private void CalculateStrain(StrainResult column, VehicleTrajectory trajectory, LoadSchema loadSchema)
        {
            var maxStrainPosition = profileYZService.GetMaxZPosition(trajectory.Center);



            //var strain = CalculateStrainInPositions(trajectory, maxStainPosition);
            //column.Strain.Add(strain);
            //column.StrainOneAuto.Add(strain);
        }

        //private double CalculateStrainInPositions(
        //    VehicleTrajectory trajectory, 
        //    double maxStainPosition)
        //{
        //    foreach (var strainPosition in maxStrainPositions)
        //    {
        //        trajectory.Left.
        //    }
        //}

        //private 

        /// <summary>
        /// ограничим максимальное количество ТС на уровне менеджера, 
        /// чтобы не повесить калькуляцию надолго, если что-то пойдёт не так
        /// </summary>
        private const int VehicleInColumnLimiter = 7;


        private Action<ColumnModel, VehicleTrajectory> GetCalculateStrainAction(
            [DisallowNull] VehicleTrajectory[] vehicleTrajectories,
            LoadSchema loadSchema,
            RoadRules roadRules)
        {
            var needToPlaceVehicles = Math.Min(roadRules.MaxAutoInColumn, VehicleInColumnLimiter);

            if (needToPlaceVehicles <= 1)
            {
                return CalculateOneVehicleCase;
            }

            if (IsIssoCrowded(loadSchema, needToPlaceVehicles))
            {
                return (ColumnModel column, VehicleTrajectory trajectory) => CalculateCrowdedCase(data, column, trajectory, positiveIntervals);
            }

            return (ColumnModel column, VehicleTrajectory trajectory) => CalculateSparseCase(column, trajectory, needToPlaceVehicles);
        }

        private bool IsIssoCrowded(LoadSchema loadSchema,
            int needToPlaceVehicles)
        {
            if (!(loadSchema?.Distance > 0 && loadSchema?.Length > 0))
            {
                return false;
            }

            var canPlaceVehicles = default(int);


            foreach (var interval in positiveIntervals)
            {
                var start = interval.X;
                var end = interval.Y;

                var hasPlaceOnTheEdges = doubleEqualityComparer.Equals(start, data.Surface.MinY) ||
                    doubleEqualityComparer.Equals(end, data.Surface.MaxY);

                var remainingDistance = end - start - data.LoadSchema.Length;

                while (remainingDistance > data.LoadSchema.Distance)
                {
                    remainingDistance -= data.LoadSchema.Distance;
                    if (remainingDistance >= data.LoadSchema.Length)
                    {
                        remainingDistance -= data.LoadSchema.Length;
                        canPlaceVehicles++;
                    }
                }

                if (hasPlaceOnTheEdges || remainingDistance >= 0)
                {
                    canPlaceVehicles++;
                }
            }

            return canPlaceVehicles <= needToPlaceVehicles;
        }

        private void CalculateSparseCase(ColumnModel column, VehicleTrajectory trajectory, int maxAutoInColumn)
        {
            var maxVehicles = Math.Min(maxAutoInColumn, VehicleInColumnLimiter);

            var heightTree = new SortedDictionary<float, float>(
                profileYZService.GetYZFromProfile(trajectory.Center)
                .Select(v => new KeyValuePair<float, float>(v.Y, v.X))
                .ToDictionary());

            var maxStrainPositions = GetMaxStrainPositions(heightTree, trajectory.Center, maxVehicles, []);

            var strain = CalculateStrainInPositions(trajectory, [.. maxStrainPositions.Select(x => (double)x)]);
            var strainOneVehicle = CalculateStrainInPositions(trajectory, [maxStrainPositions.First()]);
            column.Strain.Add(strain);
            column.StrainOneAuto.Add(strainOneVehicle);
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
