using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Parameters;
using Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions;
using Abdm.Calculation.Graphics;

namespace Abdm.Calculation.BLL
{
    public class PassTypeCalculationCoordinator (
        IPassageIntervalService passageIntervalManager,
        ISurfaceDataService surfaceDataService,
        IMeshManager meshManager,
        IRoadRulesFactory roadRulesFactory,
        IStrainCalculator strainCalculator,
        IVehicleTrajectoryService vehicleTrajectoryService,
        IPassTypeDataModelService passTypeDataModelService
        ) : IPassTypeCalculationCoordinator
    {
        private const string meshErrorMessage = "Mesh construction failed";
        private const string noIntersectionsErrorMessage = "Mesh has no intersections in given passage intervals";
        private const string passageIntervalErrorMessage = "Passage intervals for this isso have not been found";
        private const string surfaceDataNotFound = "Surface data for given isso and checkpoint was not found";
        private const string roadRulesNotFound = "Road rules for given load were not found";

        /// <summary>
        /// Коэффициент при динамическом движении на иссо
        /// TODO: ABDMP-370 - реализация сервиса расчётов динамического/статического коеффициента
        /// На самом деле он не статический
        /// </summary>
        public static double DynamicCoefficient = 1.3d;

        public List<(IPassTypeCondition condition, PassTypeEnum passType)> PassTypeConditions =
            new List<(IPassTypeCondition condition, PassTypeEnum passType)>
            {
                (new NoLimitCondition(), PassTypeEnum.NoLimit),
                (new WithoutPedestrianCondition(), PassTypeEnum.WithoutPedestian),
                (new Speed10Condition(), PassTypeEnum.MaxSpeed10),
                (new SingleAutoOnlyCondition(), PassTypeEnum.SingleAutoOnly)
            };

        public async Task<ResultExceptionContainer<PassTypeCalculationResult>> GetPassType(
            [DisallowNull] PassTypeCalculationParameters data, 
            CancellationToken cancellationToken)
        {
            var intervals = await passageIntervalManager.GetPassageIntervals(data.IssoId, 
                data.Roadway.PositionShift - data.Surface.MinY, cancellationToken);
            if (intervals?.Any() != true)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(passageIntervalErrorMessage));
            }
            var surfaceDataContainer = await surfaceDataService.GetSurfaceData(data.IssoId, data.CheckPointNumber, cancellationToken);
            //TODO: ABDMP-357 - Реализация триангуляции, если ничего не пришло. Запись новой триангуляции обратно в бд
            if (surfaceDataContainer?.Data?.Triangles == null || !surfaceDataContainer.IsSuccess)
            {
                var surfaceDataException = new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(surfaceDataNotFound));
                if (surfaceDataContainer?.Exception != null)
                {
                    surfaceDataException.AddException(surfaceDataContainer.Exception);
                }
                return surfaceDataException;
            }

            //TODO: ABDMP-371 - реализация кастомных нагрузок LoadSchema.Id, подгрузка их из бд
            var roadRulesNullable = roadRulesFactory.CreateRoadRuleStrategy(data.LoadSchema.Id);
            if (roadRulesNullable is not RoadRule[] roadRules)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(roadRulesNotFound));
            }

            var mesh = meshManager.GetMeshFromPoints(
                surfaceDataContainer.Data.Points, 
                surfaceDataContainer.Data.Triangles);
            if (mesh?.Data?.DistinctXs == null || mesh.Data.DistinctYs == null)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(meshErrorMessage));
            }

            var calculationData = passTypeDataModelService.ComposePassTypeDataModel(data, intervals, roadRules);
            foreach (var interval in intervals)
            {
                var vehicleXPositions = passageIntervalManager.CalculateVehiclePositionsIncludingWheelOffsets(
                    mesh.Data.DistinctXs,
                    interval,
                    data.LoadSchema,
                    roadRules);

                var vehicleTrajectories = vehicleTrajectoryService.GetVehicleTrajectories(vehicleXPositions,
                    mesh, data.LoadSchema.Axles);

                if (vehicleTrajectories.Length == 0)
                {
                    return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(noIntersectionsErrorMessage));
                }

                calculationData.Intervals.Where(x => x.PassageIntervalRef == interval).First().Trajectories = vehicleTrajectories;
            }

            var strainResultData = strainCalculator.GetStrainResult(calculationData, roadRules);

            var resultPassType = GetPassType(strainResultData, data.Surface, roadRules);

            var response = ComposeMessage(resultPassType, data);

            return new ResultExceptionContainer<PassTypeCalculationResult>(response);
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

        private PassTypeCalculationResult ComposeMessage(PassTypeEnum resultPassType, PassTypeCalculationParameters data)
        {
            AllowedEnum allowed = resultPassType switch
            {
                PassTypeEnum.NoLimit => AllowedEnum.Allowed,
                PassTypeEnum.WithoutPedestian 
                or PassTypeEnum.MaxSpeed10 
                or PassTypeEnum.SingleAutoOnly 
                or PassTypeEnum.SingleOnlyAndPlace => AllowedEnum.Restricted,
                PassTypeEnum.Denied => AllowedEnum.Denied,
                PassTypeEnum.Unknown or _ => AllowedEnum.Denied,
            };

            return new PassTypeCalculationResult
            {
                Allowed = allowed,
                CPNumber = data.CheckPointNumber,
                Direction = data.Direction,
                Intervals = [],
                IssoId = data.IssoId,
                PassType = resultPassType,
                LoadId = data.LoadId
            };
        }

        public PassTypeCalculationResult GetFailedResponse(PassTypeCalculationParameters? data)
        {
            if (data == null)
            {
                return new PassTypeCalculationResult
                {
                    IssoId = default,
                    CPNumber = default,
                    Allowed = AllowedEnum.Undefined,
                    Intervals = [],
                    LoadId = default,
                    Direction = default,
                    Snip = default,
                    PassType = PassTypeEnum.Unknown
                };
            }
            else
            {
                return new PassTypeCalculationResult
                {
                    IssoId = data.IssoId,
                    CPNumber = data.CheckPointNumber,
                    Allowed = AllowedEnum.Undefined,
                    Intervals = [],
                    LoadId = data.LoadId,
                    Direction = data.Direction,
                    Snip = data.Snip,
                    PassType = PassTypeEnum.Unknown
                };
            } 
        }
    }
}
