using System.Diagnostics.CodeAnalysis;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Mappers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Services.LowLevelCalculation;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;
using Abdm.Calculation.SteelConcrete.Models;
using Mapster;

namespace Abdm.Calculation.BLL.Coordinators
{
    /// <summary>
    /// Базовый координатор по прокатке ТС по поверхности влияния.
    /// </summary>
    public class BaseVehicleRollingCalculationCoordinator(IPassageIntervalService passageIntervalManager,
        ISurfaceDataService surfaceDataService,
        IMeshManager meshManager,
        IRoadRulesFactory roadRulesFactory,
        IStrainResultService strainResultService,
        ISymmetryService symmetryService,
        IMaterialService materialService,
        ICoefficientProviderFactory coefficientProviderFactory,
        IEnumerable<IProfileYZService> profileYZServices,
        IEnumerable<IVehicleStrainProvider> vehicleStrainProviders,
        IEnumerable<IVehicleTrajectoryManager> vehicleTrajectoryManagers
        ) : IBaseVehicleRollingCalculationCoordinator
    {
        private const string meshErrorMessage = "Mesh construction failed";
        private const string noIntersectionsErrorMessage = "Mesh has no intersections in given passage intervals";
        private const string passageIntervalErrorMessage = "Passage intervals for this isso have not been found";
        private const string surfaceDataNotFoundErrorMessage = "Surface data for given isso and checkpoint was not found";
        private const string roadRulesNotFoundErrorMessage = "Road rules for given load were not found";

        public async Task<ResultMonad<VehicleRollingBigModel>> PrepareDataModel(
            [DisallowNull] PassTypeCalculationParameters data,
            bool? IsMirroredByZ,
            CancellationToken cancellationToken)
        {
            var intervals = await passageIntervalManager.GetPassageIntervals(data.IssoId,
                data.Roadway.PositionShift, cancellationToken);
            if (intervals?.Any() != true)
            {
                return new ResultMonad<VehicleRollingBigModel>(new Exception(passageIntervalErrorMessage));
            }
            var surfaceDataContainer = await surfaceDataService.GetSurfaceData(data.IssoId, data.CheckPointNumber, intervals, cancellationToken);

            if (surfaceDataContainer?.Result?.Triangles == null || !surfaceDataContainer.IsSuccess)
            {
                var surfaceDataException = new ResultMonad<VehicleRollingBigModel>(new Exception(surfaceDataNotFoundErrorMessage));
                if (surfaceDataContainer?.Exception != null)
                {
                    surfaceDataException.AddException(surfaceDataContainer.Exception);
                }
                return surfaceDataException;
            }

            var roadRulesNullable = roadRulesFactory.CreateRoadRuleStrategy(data.LoadSchema.Type, data.LoadSchema.Id);
            if (roadRulesNullable is not RoadRule[] roadRules)
            {
                return new ResultMonad<VehicleRollingBigModel>(new Exception(roadRulesNotFoundErrorMessage));
            }

            var dataModel = data.Adapt<VehicleRollingSmallModel>();
            DataModelFixer.Fix(dataModel, surfaceDataContainer.Result, data);
            dataModel.Load.IsSymmetric = symmetryService.IsLoadSymmetric(dataModel.Load);
            dataModel.Load.ActualDirection = symmetryService.CalculateDirection(dataModel.Load.IsSymmetric, data.Direction);
            dataModel.Load.WheelOffsetsMap = PassTypeFormulas.DistanceBetweenTrajectoryCenterAndAxles(dataModel.Load.Axles);
            dataModel.Load.MassCenterPosition = PassTypeFormulas.MassCenterPosition(dataModel.Load.Axles);
            dataModel.Surface.StrainCalculationGroupType = surfaceDataContainer.Result.StrainCalculationType.Map();
            dataModel.Surface.StrainCalculationType = surfaceDataContainer.Result.StrainCalculationType;
            dataModel.Surface.StrainTypeSpecificData = surfaceDataContainer.Result.StrainTypeSpecificData;
            dataModel.CoefficientProvider = coefficientProviderFactory.GetStrainProvider(data.Snip, dataModel.Load.Type);
            if (dataModel.Surface.StrainTypeSpecificData is SteelConcreteData steelConcreteData)
            {
                steelConcreteData.SteelConcreteParameters = data.Surface.Adapt<IssoSteelConcreteParameters>();
            }
            dataModel.Surface.Material = await materialService.GetMaterial(
                (int)data.IssoId,
                surfaceDataContainer.Result.SubstructureId,
                surfaceDataContainer.Result.CheckPointType, 
                cancellationToken);

            var mesh = meshManager.GetMeshFromPoints(
                surfaceDataContainer.Result.Points,
                surfaceDataContainer.Result.Triangles,
                out Vector3I[]? trianglesToCache,
                IsMirroredByZ == true);
            if (mesh?.Data?.DistinctXs == null)
            {
                return new ResultMonad<VehicleRollingBigModel>(new Exception(meshErrorMessage));
            }

            var secondaryMesh = IsMirroredByZ == null
                ? meshManager.GetMeshFromPoints(
                surfaceDataContainer.Result.Points,
                trianglesToCache ?? surfaceDataContainer.Result.Triangles,
                out _,
                true)
                : null;

            return new ResultMonad<VehicleRollingBigModel>(new VehicleRollingBigModel
            {
                Data = dataModel,
                Intervals = intervals,
                RoadRules = roadRules,
                Mesh = mesh,
                SecondaryMesh = secondaryMesh,
                TrianglesToCache = trianglesToCache,
            });
        }

        public ResultMonad<VehicleRollingResult> RollAndGetStrainResult(
            [DisallowNull] VehicleRollingBigModel dataModel,
            CancellationToken cancellationToken)
        {
            var intervalModels = new List<IntervalModel>();

            var doSlabCalculation = dataModel.Data.Surface.StrainCalculationGroupType == Enums.StrainCalculationGroupTypeEnum.Slab;
            var requiredTrafficJamStrainCalculaton = dataModel.RoadRules.Any(r => r.DoTrafficJamLoadCalculation);

            IProfileYZService profileYZService;
            IVehicleTrajectoryManager vehicleTrajectoryManager;
            IVehicleStrainProvider vehicleStrainProvider;
            if (doSlabCalculation || requiredTrafficJamStrainCalculaton)
            {
                profileYZService = profileYZServices.Where(x => x is ProfileYZServiceVolumetric).First();
                vehicleTrajectoryManager = vehicleTrajectoryManagers.Where(x => x is VehicleTrajectoryManagerVolumetric).First();
                vehicleStrainProvider = vehicleStrainProviders.Where(x => x is VehicleStrainProviderVolumetric).First();
                ((VehicleStrainProviderVolumetric)vehicleStrainProvider).DoWheelStrainCalcVolumetric = doSlabCalculation;
            }
            else
            {
                profileYZService = profileYZServices.Where(x => x is ProfileYZService).First();
                vehicleTrajectoryManager = vehicleTrajectoryManagers.Where(x => x is VehicleTrajectoryManager).First();
                vehicleStrainProvider = vehicleStrainProviders.Where(x => x is VehicleStrainProvider).First();
            }
            dataModel.Data.VehicleStrainProvider = vehicleStrainProvider;
            if (!doSlabCalculation)
            {
                dataModel.Data.Surface.RoadCoatSize = 0;
            }

            foreach (var interval in dataModel.Intervals)
            {
                var intervalModel = vehicleTrajectoryManager.GetIntervalModel(dataModel, interval, doSlabCalculation, profileYZService);
                if (intervalModel.Trajectories?.Any() != true)
                {
                    return new ResultMonad<VehicleRollingResult>(new Exception(noIntersectionsErrorMessage));
                }
                intervalModels.Add(intervalModel);
            }

            var strainResults = strainResultService.GetStrainResults(dataModel, intervalModels);

            return new ResultMonad<VehicleRollingResult>(new VehicleRollingResult() { 
                DataModel = dataModel, 
                StrainResults = strainResults
            });
        }
    }
}
