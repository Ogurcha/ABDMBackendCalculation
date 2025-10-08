using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models.Algorithmic;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.BLL.Models.Parameters;
using Mapster;

namespace Abdm.Calculation.BLL.Services
{
    public class PassTypeDataModelService : IPassTypeDataModelService
    {
        private const double DefaultVehicleWidth = 3;
        private const double DefaultVehicleLength = 4.5d;
        private const double DefaultVehicleDistance = 3d;

        public PassTypeDataModel ComposePassTypeDataModel(
            PassTypeCalculationParameters inputData,
            PassageInterval[] passageIntervals,
            RoadRule[] roadRules)
        {
            var loadModel = new LoadModel()
            {
                Direction = inputData.Direction,
                Width = inputData.LoadSchema.Width ?? DefaultVehicleWidth,
                Length = inputData.LoadSchema.Length ?? DefaultVehicleLength,
                Axles = inputData.LoadSchema.Axles
            };
            


            var intervals = new List<IntervalModel>(roadRules.SelectMany(x => GetIntervalModels(x, passageIntervals, inputData)));

            return new PassTypeDataModel()
            {
                Surface = inputData.Surface.Adapt<SurfaceModel>(),
                Load = loadModel,
                Intervals = intervals
            };
        }

        private IEnumerable<IntervalModel> GetIntervalModels(
            RoadRule roadRule,
            PassageInterval[] passageIntervals,
            PassTypeCalculationParameters inputData) =>
            passageIntervals.Select(i =>
            {
                return new IntervalModel(i, roadRule)
                {
                    VehicleDistance = Math.Max(inputData.LoadSchema.Distance ?? DefaultVehicleDistance, roadRule.MinColumnDistance),
                    DistanceForSafetyLineLeft = roadRule.HasSafetyLine ? i.SafetyLineLeft : 0,
                    DistanceForSafetyLineRight = roadRule.HasSafetyLine ? i.SafetyLineRight : 0,
                    AbsolutePositionLeft = roadRule.HasSafetyLine ? i.AbsolutePositionLeft + i.SafetyLineLeft : i.AbsolutePositionLeft,
                    AbsolutePositionRight = roadRule.HasSafetyLine ? i.AbsolutePositionRight - i.SafetyLineLeft : i.AbsolutePositionLeft,
                    LaneCount = Math.Min(i.LaneCount, roadRule.MaxColumnCount)
                };
            });
    }
}
