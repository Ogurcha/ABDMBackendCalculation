using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.Graphics;
using Mapster;

namespace Abdm.Calculation.BLL
{
    public class PassTypeCalculationCoordinator (
        IPassageIntervalService passageIntervalManager,
        ISurfaceDataService surfaceDataService,
        IMeshManager meshManager,
        IRoadRulesFactory roadRulesFactory,
        IStrainResultService strainResultService,
        IVehicleTrajectoryService vehicleTrajectoryService,
        IPassTypeResolverFactory passTypeResolverFactory,
        ISymmetryService symmetryService,
        IMaterialService materialService
        ) : IPassTypeCalculationCoordinator
    {
        private const string meshErrorMessage = "Mesh construction failed";
        private const string noIntersectionsErrorMessage = "Mesh has no intersections in given passage intervals";
        private const string passageIntervalErrorMessage = "Passage intervals for this isso have not been found";
        private const string surfaceDataNotFoundErrorMessage = "Surface data for given isso and checkpoint was not found";
        private const string roadRulesNotFoundErrorMessage = "Road rules for given load were not found";
        private const string passTypeResolverNotFoundErrorMessage = "Pass type resolver for given load were not found";
        private const string strainIsNaNErrorMessage = "Calculation error. Strain equals Double.NaN";

        public async Task<ResultExceptionContainer<PassTypeCalculationResult>> GetPassType(
            [DisallowNull] PassTypeCalculationParameters data, 
            CancellationToken cancellationToken)
        {
            var intervals = await passageIntervalManager.GetPassageIntervals(data.IssoId, 
                data.Roadway.PositionShift, cancellationToken);
            if (intervals?.Any() != true)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(passageIntervalErrorMessage));
            }
            var surfaceDataContainer = await surfaceDataService.GetSurfaceData(data.IssoId, data.CheckPointNumber, intervals, cancellationToken);
            //TODO: ABDMP-357 - Реализация триангуляции, если ничего не пришло. Запись новой триангуляции обратно в бд
            if (surfaceDataContainer?.Data?.Triangles == null || !surfaceDataContainer.IsSuccess)
            {
                var surfaceDataException = new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(surfaceDataNotFoundErrorMessage));
                if (surfaceDataContainer?.Exception != null)
                {
                    surfaceDataException.AddException(surfaceDataContainer.Exception);
                }
                return surfaceDataException;
            }

            //TODO: ABDMP-371 - реализация кастомных нагрузок LoadSchema.Id, подгрузка их из бд
            var roadRulesNullable = roadRulesFactory.CreateRoadRuleStrategy(data.LoadSchema.Type, data.LoadSchema.Id);
            if (roadRulesNullable is not RoadRule[] roadRules)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(roadRulesNotFoundErrorMessage));
            }

            var dataModel = data.Adapt<PassTypeSmallModel>();
            DataModelFixer.Fix(dataModel, surfaceDataContainer.Data, data);
            dataModel.Load.IsSymmetric = symmetryService.IsLoadSymmetric(dataModel.Load);
            dataModel.Surface.StrainCalculationGroupType = surfaceDataContainer.Data.StrainCalculationType.Map();
            dataModel.Surface.StrainTypeSpecificData = surfaceDataContainer.Data.StrainTypeSpecificData;
            dataModel.Surface.Material = await materialService.GetMaterial(data, surfaceDataContainer.Data.CheckPointType, cancellationToken);
            var mesh = meshManager.GetMeshFromPoints(
                surfaceDataContainer.Data.Points, 
                surfaceDataContainer.Data.Triangles,
                dataModel.Surface.IsMirroredByZ);
            if (mesh?.Data?.DistinctXs == null || mesh.Data.DistinctYs == null)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(meshErrorMessage));
            }

            var intervalModels = new List<IntervalModel>();
            foreach (var interval in intervals)
            {
                var intervalModel = vehicleTrajectoryService.GetIntervalModel(dataModel, mesh, interval, roadRules);
                if (intervalModel.Trajectories?.Any() != true)
                {
                    return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(noIntersectionsErrorMessage));
                }
                intervalModels.Add(intervalModel);
            }

            var strainResults = strainResultService.GetStrainResults(dataModel, intervalModels, roadRules, mesh);
            if (strainResults.Any(x => x.Strain == Double.NaN))
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(strainIsNaNErrorMessage));
            }
            var ptr = passTypeResolverFactory.GetPassTypeResolver(dataModel.Surface.StrainCalculationGroupType);
            if (ptr == null)
            {
                return new ResultExceptionContainer<PassTypeCalculationResult>(new Exception(passTypeResolverNotFoundErrorMessage));
            }
            var resultPassType = ptr.Resolve(strainResults, dataModel.Surface);

            var response = ComposeMessage(resultPassType, data);

            return new ResultExceptionContainer<PassTypeCalculationResult>(response);
        }

        private PassTypeCalculationResult ComposeMessage(PassTypeEnum resultPassType, PassTypeCalculationParameters data)
        {
            AllowedEnum allowed = resultPassType switch
            {
                PassTypeEnum.NoLimit => AllowedEnum.Allowed,
                PassTypeEnum.WithoutPedestrian 
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
